import { useEffect, useRef, useState } from "react";
import CheckoutPage from "./CheckoutPage";
import { useCreateOrderMutation } from "../order/OrderApi";
import { useCreatePaymentIntentMutation } from "./paymentApi";
import { useAppDispatch, useAppSelector } from "../../app/store/store";
import LoadingIndicator from "../../app/layout/LoadingIndicator";
import { clearCart } from "./CartSlice";

export default function CheckoutWrapper() {
  const cartItems = useAppSelector((state) => state.cart.items);
  const { id } = useAppSelector((state) => state.auth.user!);
  const [orderId, setOrderId] = useState<number | null>(null);
  const [clientSecret, setClientSecret] = useState<string | null>(null);
  const dispatch = useAppDispatch();

  // ✅ Prevent double initialization in dev (React strict mode)
  const hasInitialized = useRef(false);

  const [createOrder, { isLoading: isOrderLoading }] = useCreateOrderMutation();
  const [createPaymentIntent, { isLoading: isPaymentLoading }] =
    useCreatePaymentIntentMutation();

  useEffect(() => {
    const initPayment = async () => {
      // Guard — prevent multiple creations
      if (hasInitialized.current) return;
      hasInitialized.current = true;

      try {
        if (!cartItems?.length || !id) {
          console.warn("No cart items or user ID available.");
          return;
        }

        // Create the order first
        const orderPayload = {
          items: cartItems.map((item) => ({
            courseId: item.courseId,
            title: item.title,
            price: item.price,
            quantity: item.quantity,
            subject: item.subject,
            gradeLevel: item.gradeLevel,
            teacherName: item.teacherName,
            description: item.description,
          })),
          userId: id,
          currency: "usd",
        };

        const createdOrder = await createOrder(orderPayload).unwrap();
        setOrderId(createdOrder.orderId);

        //  Then create the Stripe PaymentIntent
        const paymentPayload = {
          orderId: createdOrder.orderId,
          currency: "usd",
          items: cartItems,
          userId: id,
        };

        const payment = await createPaymentIntent(paymentPayload).unwrap();
        console.log("✅ PaymentIntent created:", payment);

        setClientSecret(payment.clientSecret);

        // Clear cart after successful order creation
        dispatch(clearCart());
      } catch (err) {
        console.error("❌ Payment initialization failed:", err);
        // Allow retry on failure
        hasInitialized.current = false;
      }
    };

    // Run only once per checkout flow
    if (!hasInitialized.current && cartItems.length > 0 && !orderId && !clientSecret) {
      initPayment();
    }
  }, [cartItems, id, orderId, clientSecret, createOrder, createPaymentIntent, dispatch]);

  // Loading indicator
  if (isOrderLoading || isPaymentLoading)
    return <LoadingIndicator variant="spinner" size="lg" colorClass="white-text" />;

  // Preparing state
  if (!clientSecret || !orderId)
    return <div className="text-center text-gray-500">Preparing checkout...</div>;

  // Ready for checkout
  return <CheckoutPage clientSecret={clientSecret} orderId={orderId} />;
}
