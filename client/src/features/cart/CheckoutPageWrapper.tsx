import { useEffect, useState } from "react";
import CheckoutPage from "./CheckoutPage";
import { useCreateOrderMutation } from "../order/OrderApi";
import { useCreatePaymentIntentMutation } from "./paymentApi";
import { useAppSelector } from "../../app/store/store";
import LoadingIndicator from "../../app/layout/LoadingIndicator";



export default function CheckoutWrapper() {
  const cartItems = useAppSelector((state) => state.cart.items);
  const { id } = useAppSelector((state) => state.auth.user!);
  const [orderId, setOrderId] = useState<number | null>(null);
  const [clientSecret, setClientSecret] = useState<string | null>(null);

  const [createOrder, { isLoading: isOrderLoading }] = useCreateOrderMutation();
  const [createPaymentIntent, { isLoading: isPaymentLoading }] = useCreatePaymentIntentMutation();

  useEffect(() => {
    const initPayment = async () => {
      try {
        // 1️⃣ Create the order first
        const orderPayload = {
          items: cartItems.map(item => ({
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

        // 2️⃣ Then create the Stripe PaymentIntent
        const paymentPayload = {
          orderId: createdOrder.orderId,
          currency: "usd",
          items: cartItems,
          userId: id,
        };

        const payment = await createPaymentIntent(paymentPayload).unwrap();
        console.log("PaymentIntent created:", payment);
        setClientSecret(payment.clientSecret);
      } catch (err) {
        console.error("Payment init failed:", err);
      }
    };

    if (cartItems.length > 0) initPayment();
  }, [cartItems, createOrder, createPaymentIntent]);

  if (isOrderLoading || isPaymentLoading) 
    return <LoadingIndicator variant="spinner" size="lg" colorClass="white-text" />;

  if (!clientSecret || !orderId)
    return <div className="text-center text-gray-500">Preparing checkout...</div>;

  return <CheckoutPage clientSecret={clientSecret} orderId={orderId} />;
}
