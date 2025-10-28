import { useNavigate } from "react-router-dom"
import { useAppSelector } from "../../app/store/store";
import { ShoppingCart } from "lucide-react";

const CartIcon = () => {
    const navigate = useNavigate();
    const itemCount = useAppSelector(state => state.cart.cart?.items.reduce((sum, curr) => sum + curr.quantity, 0));

    if (itemCount === undefined)return;
  return (
    <button
    onClick={() => navigate("/cartPage")}
    className="relative flex items-center justify-center p-2 rounded-full hover:bg-grey-100 transition"
    >
      <ShoppingCart size={24} />
      {itemCount > 0 && (
        <span className="absolute -top-1 -right-1 bg-blue-600 text-white text-xs rounded-full w-5 h-5 flex items-center justify-center" >
            {itemCount}

        </span>
      )}
    </button>
  )
}

export default CartIcon;
