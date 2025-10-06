import { useNavigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../app/store/store";
import { clearCart, removeItemFromCart, updateQuantity } from "./CartSlice";

export default function CartPage() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { items } = useAppSelector((state) => state.cart);

  const total = items.reduce((sum, item) => sum + item.price * item.quantity, 0);

  if (items.length === 0) {
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

  return (
    <div className="max-w-5xl mx-auto py-10 px-4">
      <h1 className="text-3xl font-bold mb-6">Your Cart</h1>

      <div className="space-y-6">
        {items.map((item) => (
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
                <input
                  type="number"
                  min={1}
                  value={item.quantity}
                  onChange={(e) =>
                    dispatch(updateQuantity({ courseId: item.courseId, quantity: Number(e.target.value) }))
                  }
                  className="w-16 border rounded px-2 py-1"
                />
              </div>
            </div>

            <div className="text-right">
              <p className="text-lg font-semibold">${item.price.toFixed(2)}</p>
              <button
                onClick={() => dispatch(removeItemFromCart(item.courseId))}
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
          onClick={() => dispatch(clearCart())}
          className="text-gray-500 hover:underline"
        >
          Clear Cart
        </button>
        <div className="text-right">
          <p className="text-2xl font-semibold mb-4">Total: ${total.toFixed(2)}</p>
          <button
            onClick={() => navigate("/checkout")}
            className="bg-green-600 text-white px-6 py-3 rounded-lg hover:bg-green-700 transition"
          >
            Proceed to Checkout
          </button>
        </div>
      </div>
    </div>
  );
}
