import { Outlet } from "react-router-dom"
import { store, useAppSelector } from "../store/store"
import Navbar from "./Navbar";
import { setCredentials } from "../../features/auth/authSlice";
import { useEffect } from "react";

function App() {
 const studentId = useAppSelector(state => state.auth.user?.id); 

 useEffect(() => {
   const storedAuth = localStorage.getItem("auth");
    if (storedAuth) {
   store.dispatch(setCredentials(JSON.parse(storedAuth)));
   }
 }, [])



  return (
    <div className="min-h-screen bg-gradient-to-br from-yellow-200 via-pink-200 to-indigo-200">

      <Navbar />

      {/* Main content */}
      <div className="p-4">
        <Outlet context={{studentId}}/>
      </div>
    </div>
  );
}

export default App
