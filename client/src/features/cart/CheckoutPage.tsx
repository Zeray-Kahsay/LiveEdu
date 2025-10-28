import { Elements } from "@stripe/react-stripe-js";
import { loadStripe } from "@stripe/stripe-js";
import CheckoutForm from "./CheckoutForm";
import { useLocation } from "react-router-dom";

export default function CheckoutPage() {
  const location = useLocation();
  const { clientSecret, publishableKey } = location.state || {};

  if (!clientSecret || !publishableKey)
    return (
      <p className="text-center mt-8 text-red-600">
        Missing payment details. Please start checkout again.
      </p>
    );

  const stripePromise = loadStripe(publishableKey);
  const options = { clientSecret };

  return (
    <Elements stripe={stripePromise} options={options}>
      <div className="p-6 bg-white shadow-md rounded-md">
        <h2 className="text-xl font-semibold mb-4 text-center">
          Complete Your Payment
        </h2>
        <CheckoutForm clientSecret={clientSecret} />
      </div>
    </Elements>
  );
}
