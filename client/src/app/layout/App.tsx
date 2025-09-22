import { Outlet } from "react-router-dom"
//import LoadingIndicator from "./LoadingIndicator"
import { store } from "../store/store"
import Navbar from "./Navbar";
import { setCredentials } from "../../features/auth/authSlice";

function App() {
 // const { isLoading } = useAppSelector(state => state.ui);

  const storedAuth = localStorage.getItem("auth");
   if (storedAuth) {
  store.dispatch(setCredentials(JSON.parse(storedAuth)));
  }


  return (
    <div className="min-h-screen bg-gradient-to-br from-yellow-200 via-pink-200 to-indigo-200">
      

      <Navbar />
      {/* {isLoading && (
        <div className="absolute top-4 right-4 z-50">
          <LoadingIndicator variant="dots" size="lg" colorClass="text-indigo-600" /> 
        </div>
      )} */}

      {/* Main content */}
      <div className="p-4">
        <Outlet />
      </div>
    </div>
  );
}

export default App
