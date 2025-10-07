import { useEffect, useState } from "react";
import {
  useStripe,
  useElements,
  PaymentElement,
} from "@stripe/react-stripe-js";
import { useAppSelector } from "../../app/store/store";
import { useCreatePaymentIntentMutation } from "./paymentApi";
import { Button } from "../../app/layout/ui/Button";

const CheckoutForm = () => {
  const stripe = useStripe();
  const elements = useElements();
  const cart = useAppSelector((state) => state.cart);
  const [createPaymentIntent, { isLoading }] = useCreatePaymentIntentMutation();

  const [clientSecret, setClientSecret] = useState<string | null>(null);
  const [status, setStatus] = useState<string>("");

  useEffect(() => {
    const initPayment = async () => {
      try {
        const res = await createPaymentIntent(cart).unwrap();
        setClientSecret(res.clientSecret);
      } catch (err) {
        console.error("Error creating payment intent:", err);
      }
    };
    initPayment();
  }, [cart, createPaymentIntent]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!stripe || !elements || !clientSecret) return;

    setStatus("Processing...");

    const result = await stripe.confirmPayment({
      elements,
      confirmParams: {
        return_url: `${window.location.origin}/payment-success`,
      },
    });

    if (result.error) {
      setStatus(result.error.message || "Payment failed");
    } else {
      setStatus("Payment successful!");
    }
  };

  if (!clientSecret) return <div>Initializing payment...</div>;

  return (
    <form onSubmit={handleSubmit} className="max-w-md mx-auto mt-8 space-y-4">
      <PaymentElement />
      <Button type="submit" disabled={!stripe || isLoading}>
        {isLoading ? "Processing..." : "Pay Now"}
      </Button>
      {status && <p className="text-center text-sm text-gray-500">{status}</p>}
    </form>
  );
};

export default CheckoutForm;