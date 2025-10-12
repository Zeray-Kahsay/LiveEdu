import { Elements } from "@stripe/react-stripe-js";
import { loadStripe } from "@stripe/stripe-js";
import CheckoutForm from "./CheckoutForm";

interface checkoutPageProps  {
  clientSecret: string;
  orderId: number;
};

const stripePromise = loadStripe(import.meta.env.VITE_STRIPE_PK!);

export default function CheckoutPage({ clientSecret, orderId }: checkoutPageProps) {
  // Customize the appearance of the payment element
  const options = {
    clientSecret,
    appearance: {
      theme: "stripe" as "stripe", 
    },
  };

  return (
    <Elements stripe={stripePromise} options={options}>
      <CheckoutForm orderId={orderId} />
    </Elements>
  );
}
