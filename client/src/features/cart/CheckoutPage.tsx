
import { useNavigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../app/store/store";
import { clearCart } from "./CartSlice";

const CheckoutPage = () => {
  const { items } = useAppSelector((s) => s.cart);
  const { user } = useAppSelector((s) => s.auth);
  const dispatch = useAppDispatch();
  const navigate = useNavigate();


 // I have protected rotues already and will place it inside REQUIREAUTH
  if (!user) {
    return (
      <div className="max-w-2xl mx-auto mt-12 text-center">
        <h2 className="text-xl font-semibold text-gray-700">You must be logged in to checkout.</h2>
        <button
          onClick={() => navigate("/login")}
          className="mt-4 px-6 py-3 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700"
        >
          Login
        </button>
      </div>
    );
  }

  if (items.length === 0) {
    return (
      <div className="max-w-2xl mx-auto mt-12 text-center">
        <h2 className="text-xl font-semibold text-gray-700">Your cart is empty.</h2>
        <button
          onClick={() => navigate("/courseCatalog")}
          className="mt-4 px-6 py-3 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700"
        >
          Browse Courses
        </button>
      </div>
    );
  }

  const handlePayment = () => {
    // 🔥 Simulate successful payment
    setTimeout(() => {
      // Normally call backend → create order → add course to user
      dispatch(clearCart());
      navigate("/dashboard/my-courses", { state: { message: "Payment successful! Your courses are available." } });
    }, 1000);
  };

  return (
    <div className="max-w-2xl mx-auto mt-12 bg-white shadow-lg p-6 rounded-xl">
      <h2 className="text-2xl font-bold text-gray-800 mb-6">Checkout</h2>

      <ul className="divide-y divide-gray-200 mb-6">
        {items.map((course) => (
          <li key={course.courseId} className="py-3 flex justify-between">
            <span className="text-gray-700">{course.title}</span>
            <span className="text-gray-500 text-sm">{course.subject}</span>
          </li>
        ))}
      </ul>

      <div className="flex justify-between items-center">
        <button
          onClick={() => navigate("/cartPage")}
          className="px-4 py-2 bg-gray-100 text-gray-600 rounded-lg hover:bg-gray-200"
        >
          Back to Cart
        </button>
        <button
          onClick={handlePayment}
          className="px-6 py-3 bg-green-600 text-white rounded-lg hover:bg-green-700"
        >
          Pay Now
        </button>
      </div>
    </div>
  );
};

export default CheckoutPage;
