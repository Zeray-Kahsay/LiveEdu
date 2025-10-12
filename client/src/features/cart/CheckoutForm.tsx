import { PaymentElement, useStripe, useElements } from "@stripe/react-stripe-js";
import { useState } from "react";

interface checkoutProps{
  orderId: number;
}

export default function CheckoutForm({ orderId }: checkoutProps) {
  const stripe = useStripe();
  const elements = useElements();
  const [isProcessing, setIsProcessing] = useState(false);
  const [message, setMessage] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!stripe || !elements) return;

    setIsProcessing(true);
    setMessage(null);

    const { error } = await stripe.confirmPayment({
      elements,
      confirmParams: {
        return_url: `${window.location.origin}/order/payment-success?orderId=${orderId}`,
      },
    });

    if (error) {
      setMessage(error.message ?? "Payment failed. Please try again.");
    }

    setIsProcessing(false);
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <PaymentElement />
      <button
        type="submit"
        disabled={!stripe || isProcessing}
        className="w-full bg-blue-600 text-white p-2 rounded"
      >
        {isProcessing ? "Processing..." : "Pay now"}
      </button>
      {message && <div className="text-red-500 text-sm">{message}</div>}
    </form>
  );
}
