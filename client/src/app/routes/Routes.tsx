import { createBrowserRouter } from "react-router-dom";
import App from "../layout/App";
import HomePage from "../../features/home/HomePage";
import AboutPage from "../../features/about/AboutPage";
import ContactPage from "../../features/contact/ContactPage";
import Catalog from "../../features/catalog/Catalog";
import CourseDetails from "../../features/catalog/CourseDetails";
import RegisterForm from "../../features/auth/RegisterForm";
import LoginForm from "../../features/auth/LoginForm";
import ProfilePage from "../../features/profile/ProfilePage";
import CourseList from "../../features/course/CourseList";

export const router = createBrowserRouter([
    {
        path: '/',
        element: <App />,
        children: [
            {path: '/', element: <HomePage />},
            {path: '/about', element: <AboutPage />},
            {path: '/register', element: <RegisterForm />},
            {path: '/login', element: <LoginForm />},
            {path: '/profile', element: <ProfilePage /> },
            {path: '/courseList', element: <CourseList /> },
            {path: '/contact', element: <ContactPage />},
            {path: '/catalog', element: <Catalog />},
            {path: '/course/:id', element: <CourseDetails />},
        ]
    },
    
])
