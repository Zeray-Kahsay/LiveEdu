import { Navigate, Outlet } from "react-router-dom";
import { useAppSelector } from "../store/store"

const RequireAuth = () => {
    const user = useAppSelector(state => state.auth.user);

    console.log("RequireAuth - user:", user);

    if (!user) return <Navigate to='/login' replace />;
    
    return <Outlet context={{studentId: user.id}} />;
}

export default RequireAuth
