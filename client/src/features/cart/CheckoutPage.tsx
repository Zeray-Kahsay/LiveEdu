import { useEffect, useState } from "react";
import { useStripe, useElements, PaymentElement } from "@stripe/react-stripe-js";
import { useAppSelector } from "../../app/store/store";
import { useCreatePaymentIntentMutation } from "./paymentApi";
import { Button } from "../../app/layout/ui/Button";

export default function CheckoutPage() {
  const stripe = useStripe();
  const elements = useElements();

  const cart = useAppSelector((state) => state.cart);
  const [createPaymentIntent, { data, isLoading }] = useCreatePaymentIntentMutation();
  const [message, setMessage] = useState("");
  const id = useAppSelector((state) => state.auth.user?.id);

  useEffect(() => {
    if (cart.items.length > 0) {
      createPaymentIntent({
        items: cart.items.map((i) => ({
          courseId: i.courseId,
          title: i.title,
          price: i.price,
          quantity: i.quantity,
          teacherName: i.teacherName,
          subject: i.subject,
          gradeLevel: i.gradeLevel,
          description: i.description
        })),
        currency: "usd",
        userId: id, 
      });
    }
  }, [cart, createPaymentIntent]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!stripe || !elements || !data?.clientSecret) return;

    const { error } = await stripe.confirmPayment({
      elements,
      confirmParams: {
        return_url: `${window.location.origin}/payment-success`,
      },
    });

    if (error) {
      setMessage(error.message ?? "Payment failed");
    }
  };

  return (
    <div className="max-w-md mx-auto p-6">
      <h1 className="text-2xl font-semibold mb-4">Checkout</h1>
      {!data && <p>Preparing your payment...</p>}

      {data && (
        <form onSubmit={handleSubmit}>
          <PaymentElement />
          <Button
            type="submit"
            disabled={!stripe || isLoading}
            className="mt-4 w-full"
          >
            {isLoading ? "Processing..." : "Pay Now"}
          </Button>
        </form>
      )}

      {message && <p className="text-red-500 mt-3">{message}</p>}
    </div>
  );
}







// import { useEffect, useState } from "react";
// import { useStripe, useElements, PaymentElement } from "@stripe/react-stripe-js";
// import { useAppSelector } from "../../app/store/store";
// import { useCreatePaymentIntentMutation } from "./paymentApi";
// import { Button } from "../../app/layout/ui/Button";

// export default function CheckoutPage() {
//   const stripe = useStripe();
//   const elements = useElements();

//   const cart = useAppSelector((state) => state.cart);
//   const [createPaymentIntent, { data, isLoading }] = useCreatePaymentIntentMutation();
//   const [message, setMessage] = useState("");
//   const id = useAppSelector((state) => state.auth.user?.id);

//   useEffect(() => {
//     if (cart.items.length > 0) {
//       createPaymentIntent({
//         items: cart.items.map((i) => ({
//           courseId: i.courseId,
//           title: i.title,
//           price: i.price,
//           quantity: i.quantity,
//           teacherName: i.teacherName,
//           subject: i.subject,
//           gradeLevel: i.gradeLevel,
//           description: i.description
//         })),
//         currency: "usd",
//         userId: id, 
//       });
//     }
//   }, [cart, createPaymentIntent]);

//   const handleSubmit = async (e: React.FormEvent) => {
//     e.preventDefault();
//     if (!stripe || !elements || !data?.clientSecret) return;

//     const { error } = await stripe.confirmPayment({
//       elements,
//       confirmParams: {
//         return_url: `${window.location.origin}/payment-success`,
//       },
//     });

//     if (error) {
//       setMessage(error.message ?? "Payment failed");
//     }
//   };

//   return (
//     <div className="max-w-md mx-auto p-6">
//       <h1 className="text-2xl font-semibold mb-4">Checkout</h1>
//       {!data && <p>Preparing your payment...</p>}

//       {data && (
//         <form onSubmit={handleSubmit}>
//           <PaymentElement />
//           <Button
//             type="submit"
//             disabled={!stripe || isLoading}
//             className="mt-4 w-full"
//           >
//             {isLoading ? "Processing..." : "Pay Now"}
//           </Button>
//         </form>
//       )}

//       {message && <p className="text-red-500 mt-3">{message}</p>}
//     </div>
//   );
// }




