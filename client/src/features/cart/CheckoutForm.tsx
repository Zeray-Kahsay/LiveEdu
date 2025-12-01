import { useStripe, useElements, CardElement } from "@stripe/react-stripe-js";
import { useState, type FormEvent } from "react";
import { useAppDispatch } from "../../app/store/store";
import { clearCart } from "./CartSlice";

interface CheckoutFormProps {
  clientSecret: string;
}

export default function CheckoutForm({ clientSecret }: CheckoutFormProps) {
  const stripe = useStripe();
  const elements = useElements();
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState<string | null>(null);
  const dispatch = useAppDispatch();

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    if (!stripe || !elements) return;

    setLoading(true);
    setMessage(null);

    const result = await stripe.confirmCardPayment(clientSecret, {
      payment_method: {
        card: elements.getElement(CardElement)!,
      },
    });

    if (result.error) {
      setMessage(result.error.message ?? "Payment failed.");
    } else if (result.paymentIntent && result.paymentIntent.status === "succeeded") {
      setMessage("✅ Payment succeeded! You are now enrolled.");
      dispatch(clearCart());
      
    }

    setLoading(false);
  };

  return (
    <form onSubmit={handleSubmit} className="max-w-md mx-auto space-y-4">
      <CardElement className="p-3 border rounded-md" />
      <button
        type="submit"
        disabled={!stripe || loading}
        className="btn btn-primary w-full"
      >
        {loading ? "Processing..." : "Pay Now"}
      </button>
      {message && <p className="text-center text-sm text-gray-600">{message}</p>}
    </form>
  );
}

