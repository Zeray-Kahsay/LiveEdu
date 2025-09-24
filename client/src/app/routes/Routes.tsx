import { createBrowserRouter } from "react-router-dom";
import App from "../layout/App";
import AboutPage from "../../features/about/AboutPage";
import ContactPage from "../../features/contact/ContactPage";
import Catalog from "../../features/catalog/CourseCatalog";
import RegisterForm from "../../features/auth/RegisterForm";
import LoginForm from "../../features/auth/LoginForm";
import ProfilePage from "../../features/profile/ProfilePage";
import CourseList from "../../features/course/CourseList";
import CourseCatalog from "../../features/catalog/CourseCatalog";
import CourseDetails from "../../features/course/CourseDetails";
import Dashboard from "../../features/dashboard/Dashboard";
import RequireAuth from "./RequireAuth";

export const router = createBrowserRouter([
    {
        path: '/',
        element: <App />,
        children: [
            {path: '/', element: <CourseCatalog />},
            {path: '/about', element: <AboutPage />},
            {path: '/register', element: <RegisterForm />},
            {path: '/login', element: <LoginForm />},
            {path: '/profile', element: <ProfilePage /> },
            {path: '/courseList', element: <CourseList /> },
            {path: '/contact', element: <ContactPage />},
            {path: '/catalog', element: <Catalog />},
            {path: '/course/:id', element: <CourseDetails />},


            // Protected routes
            {
                element: <RequireAuth />,
                children: [
                    {path: "/dashboard", element: <Dashboard />}
                ]
            }
        ]
    },
    
])
