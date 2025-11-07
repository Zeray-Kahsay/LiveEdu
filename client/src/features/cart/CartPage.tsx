import { useNavigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../app/store/store";
import { clearCart} from "./CartSlice";
import { useClearCartMutation, useCreatePaymentIntentMutation, useRemoveItemFromCartMutation } from "./cartApi";

export default function CartPage() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const items  = useAppSelector((state) => state.cart.cart?.items ?? []);
  const totalPrice  = useAppSelector((state) => state.cart.cart?.total ?? 0);
  const [createPaymentIntent, { isLoading }] = useCreatePaymentIntentMutation();
  const [removeItemFromCart, {isLoading: isRemoving}] = useRemoveItemFromCartMutation();
  const [clearCart] = useClearCartMutation();
const cart = useAppSelector((state) => state.cart.cart);
const cartId = cart?.cartId;

console.log(totalPrice);


if ( items.length === 0) {
  return (
    <div className="flex flex-col items-center justify-center py-16">
        <h2 className="text-2xl font-semibold mb-4">Your cart is empty 🛒</h2>
        <button
          onClick={() => navigate("/courseList")}
          className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 transition"
          >
          Browse Courses
        </button>
      </div>
    );
  }
  
  
  const total = items.reduce((sum, item) => sum + item.price * item.quantity, 0);


const handleProceedToCheckout = async () => {
  if (!cartId) {
    alert("No active cart found");
    return;
  }

  try {
    const result = await createPaymentIntent(cartId).unwrap();
    console.log("Payment Intent result:", result);

    if (result?.clientSecret && result?.publishableKey) {
      navigate("/checkout", {
        state: {
          clientSecret: result.clientSecret,
          publishableKey: result.publishableKey,
        },
      });
    } else {
      alert("Failed to initialize payment");
    }
  } catch (err) {
    console.error("Payment intent failed:", err);
    alert("Something went wrong while creating payment intent");
  }
};




  return (
    <div className="max-w-5xl mx-auto py-10 px-4">
      <h1 className="text-3xl font-bold mb-6">Your Cart</h1>

      <div className="space-y-6">
        {items?.map((item) => (
          <div
            key={item.courseId}
            className="flex justify-between items-center border-b pb-4"
          >
            <div>
              <h2 className="font-semibold text-lg">{item.title}</h2>
              <p className="text-sm text-gray-500">{item.subject} — {item.gradeLevel}</p>
              <p className="text-sm text-gray-400">{item.teacherName}</p>
              <div className="flex items-center gap-2 mt-2">
                <label className="text-sm text-gray-500">Qty:</label>
              </div>
            </div>

            <div className="text-right">
              <p className="text-lg font-semibold">${item.price.toFixed(2)}</p>
              <button
                onClick={() => {
                  if (cart?.cartId) {
                    removeItemFromCart({ cartId: cart.cartId, courseId: item.courseId })
                  }
                } }
                className="text-red-600 text-sm hover:underline mt-2"
              >
                Remove
              </button>
            </div>
          </div>
        ))}
      </div>

      <div className="mt-10 flex justify-between items-center">
        <button
          onClick={() => {
            if (cartId){
              clearCart(cartId);
            }
          }}
          className="text-gray-500 hover:underline"
        >
          Clear Cart
        </button>
        <div className="text-right">
          <p className="text-2xl font-semibold mb-4">Total: ${total.toFixed(2)}</p>
         <button
           onClick={handleProceedToCheckout}
           disabled={isLoading}
           className="bg-green-600 text-white px-6 py-3 rounded-lg hover:bg-green-700 transition"
         >
           {isLoading ? "Processing..." : "Proceed to Checkout"}
        </button>

        </div>
      </div>
    </div>
  );
}
