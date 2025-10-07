import { useCreatePaymentIntentMutation } from "./paymentApi";
import { Elements } from "@stripe/react-stripe-js";
import CheckoutPage from "./CheckoutPage";
import { useAppSelector } from "../../app/store/store";
import type { loadStripe } from "@stripe/stripe-js";
import { useEffect } from "react";

interface CheckoutPageWrapperProps {
  stripePromise: ReturnType<typeof loadStripe>;
}

export default function CheckoutPageWrapper({ stripePromise }: CheckoutPageWrapperProps) {
  const cart = useAppSelector((state) => state.cart);
  const userId = useAppSelector((state) => state.auth.user?.id);
  const [createPaymentIntent, { data: paymentData, isLoading }] = useCreatePaymentIntentMutation();

  // fetch client secret when cart has items
  useEffect(() => {
    if (cart.items.length > 0 && userId) {
      createPaymentIntent({
        items: cart.items,
        currency: "usd",
        userId,
      });
    }
  }, [cart, userId, createPaymentIntent]);

  if (!paymentData?.clientSecret) return <p>Preparing your payment...</p>;

  return (
    <Elements stripe={stripePromise} options={{ clientSecret: paymentData.clientSecret }}>
      <CheckoutPage />
    </Elements>
  );
}
