import { useParams, useNavigate } from "react-router-dom";
import { BookOpen } from "lucide-react";
import {toast} from "react-toastify";
import { useAppDispatch, useAppSelector } from "../../app/store/store";
import { useGetCourseByIdQuery } from "./courseApi";
import LoadingIndicator from "../../app/layout/LoadingIndicator";
import EmptyState from "../../app/layout/ui/EmptyState";
import { Button } from "../../app/layout/ui/Button";
import { setCart } from "../cart/CartSlice"; 
import { useAddItemToCartMutation } from "../cart/cartApi";

const CourseInfo = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { user } = useAppSelector((s) => s.auth);
  const cartId  = useAppSelector((s) => s.cart.cart?.cartId); 
  const dispatch = useAppDispatch();

  const { data: course, isLoading, isError } = useGetCourseByIdQuery(Number(id));
  const [addItemToCart, { isLoading: isAdding }] = useAddItemToCartMutation();

  const handleEnroll = async () => {
    if (!user) {
      navigate("/login");
      return;
    }

    if (!course) return;

    try {
      const result = await addItemToCart({
        courseId: course.id,
        cartId: cartId || localStorage.getItem("cartId") || undefined,
      }).unwrap();

      // Update redux with new cart data from backend
      dispatch(setCart(result));
      localStorage.setItem("cart", JSON.stringify(result));
      localStorage.setItem("cartId", result.cartId)

      toast.success(`Added ${course.title} to cart!`);

    } catch (error) {
      console.error("Error adding item to cart:", error);
      toast.error("Could not add course to cart");
    }
  };

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingIndicator
          variant="spinner"
          size="lg"
          colorClass="text-indigo-600"
          fullPage
        />
      </div>
    );
  }

  if (isError || !course) {
    return (
      <EmptyState
        icon={<BookOpen className="w-12 h-12 text-indigo-500" />}
        title="Course not found"
        description="The course you are looking for does not exist."
      />
    );
  }

  return (
    <div className="max-w-4xl mx-auto p-6 bg-white shadow-md rounded-xl text-center">
      <h1 className="text-2xl font-bold text-gray-800">{course.title}</h1>
      <p className="text-gray-600 mt-2">{course.description}</p>

      <div className="mt-4 flex items-center gap-4 text-sm text-gray-500">
        <span className="px-3 py-1 bg-gray-100 rounded-full">
          {course.subject}
        </span>
        <span className="px-3 py-1 bg-indigo-100 text-indigo-700 rounded-full">
          {course.gradeLevel}
        </span>
      </div>

      <div className="mt-6">
        <Button
          onClick={handleEnroll}
          disabled={isAdding}
          className="w-2xs font-serif"
        >
          {isAdding ? "Enrolling..." : "Enroll"}
        </Button>
      </div>
    </div>
  );
};

export default CourseInfo;
