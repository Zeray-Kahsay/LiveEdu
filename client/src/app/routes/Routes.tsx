import { createBrowserRouter } from "react-router-dom";
import App from "../layout/App";
import AboutPage from "../../features/about/AboutPage";
import ContactPage from "../../features/contact/ContactPage";
import RegisterForm from "../../features/auth/RegisterForm";
import LoginForm from "../../features/auth/LoginForm";
import ProfilePage from "../../features/profile/ProfilePage";
import CourseCatalog from "../../features/catalog/CourseCatalog";
import CourseDetails from "../../features/course/CourseDetails";
import Dashboard from "../../features/dashboard/Dashboard";
import RequireAuth from "./RequireAuth";
import CourseInfo from "../../features/course/CourseInfo";
import CartPage from "../../features/cart/CartPage";
import NotFoundPage from "../layout/ui/NotFoundPage";
import SuccessPage from "../layout/ui/SuccessPage";
import ErrorPage from "../layout/ui/ErrorPage";
import CreateCourseForm from "../../features/course/CreateCourseForm";
import CheckoutPage from "../../features/cart/CheckoutPage";


export const router = createBrowserRouter([
    {
        path: '/',
        element: <App />,
        children: [
            {path: '/', element: <CourseCatalog />},
            {path: '/catalog', element: <CourseCatalog />},
            {path: '/about', element: <AboutPage />},
            {path: '/register', element: <RegisterForm />},
            {path: '/login', element: <LoginForm />},
            {path: '/profile', element: <ProfilePage /> },
            {path: '/courseList', element: <CourseCatalog /> },
            {path: '/contact', element: <ContactPage />},
            {path: '/dashboard/course/:id', element: <CourseDetails />},
            {path: '/courses/:id', element: <CourseInfo />},
            {path: "order/payment-success", element: <SuccessPage /> },
            {path: "/payment-failed", element: <ErrorPage /> },
            {path: "/add-course", element: <CreateCourseForm />},
            {path: '*', element: <NotFoundPage />},


            // Protected routes
            {
                element: <RequireAuth />,
                children: [
                    {path: "/dashboard", element: <Dashboard />},
                    {path: "/cartPage", element: <CartPage /> },
                    {path: "/checkout", element: <CheckoutPage />},


                ]
            }
        ]
    },
    
])
