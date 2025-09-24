import { useState } from "react";
import { Link } from "react-router-dom";
import { useAppSelector, useAppDispatch } from "../store/store";

import { Menu, X } from "lucide-react"; // hamburger and X icons
import { logout } from "../../features/auth/authSlice";

export default function Navbar() {
  const [isOpen, setIsOpen] = useState(false);
  const { user } = useAppSelector((state) => state.auth);
  const dispatch = useAppDispatch();

  const toggleMenu = () => setIsOpen(!isOpen);
  const handleLogout = () => {
    dispatch(logout());
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
  };

  return (
    <nav className="bg-gradient-to-r from-indigo-400 via-purple-400 to-pink-400 shadow-md">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between items-center h-16">
          {/* Logo */}
          <Link to="/"  className="bg-indigo-600 hover:bg-indigo-700 text-white font-bold text-lg py-2 px-4 rounded shadow-md transition duration-300 font-serif">
            🎓 Your Live Mentor
          </Link>

          {/* Hamburger for mobile */}
          <div className="flex md:hidden">
            <button
              onClick={toggleMenu}
              className="text-white focus:outline-none"
            >
              {isOpen ? <X size={28} /> : <Menu size={28} />}
            </button>
          </div>

          {/* Desktop Menu */}
          <div className="hidden md:flex space-x-6 items-center">
            <Link to="/dashboard" className="text-white hover:font-semibold">
              Courses
            </Link>
            {user ? (
              <>
                <Link to="/profile" className="text-white hover:font-semibold">
                  Profile
                </Link>
                <button
                  onClick={handleLogout}
                  className="bg-yellow-300 text-indigo-800 px-3 py-1 rounded-lg font-semibold hover:bg-yellow-400"
                >
                  Logout
                </button>
              </>
            ) : (
              <>
                <Link
                  to="/login"
                  className="bg-white text-indigo-700 px-3 py-1 rounded-lg font-semibold hover:bg-gray-100"
                >
                  Login
                </Link>
                <Link
                  to="/register"
                  className="bg-yellow-300 text-indigo-800 px-3 py-1 rounded-lg font-semibold hover:bg-yellow-400"
                >
                  Register
                </Link>
              </>
            )}
          </div>
        </div>
      </div>

      {/* Mobile Dropdown Menu */}
      {isOpen && (
        <div className="md:hidden bg-indigo-300 px-4 pt-2 pb-4 space-y-2">
          <Link
            to="/dashboard"
            className="block text-white font-medium"
            onClick={toggleMenu}
          >
            Courses
          </Link>
          {user ? (
            <>
              <Link
                to="/profile"
                className="block text-white font-medium"
                onClick={toggleMenu}
              >
                Profile
              </Link>
              <button
                onClick={() => {
                  handleLogout();
                  toggleMenu();
                }}
                className="w-full text-left bg-yellow-300 text-indigo-800 px-3 py-1 rounded-lg font-semibold hover:bg-yellow-400"
              >
                Logout
              </button>
            </>
          ) : (
            <>
              <Link
                to="/login"
                className="block bg-white text-indigo-700 px-3 py-1 rounded-lg font-semibold hover:bg-gray-100"
                onClick={toggleMenu}
              >
                Login
              </Link>
              <Link
                to="/register"
                className="block bg-yellow-300 text-indigo-800 px-3 py-1 rounded-lg font-semibold hover:bg-yellow-400"
                onClick={toggleMenu}
              >
                Register
              </Link>
            </>
          )}
        </div>
      )}
    </nav>
  );
}


{/* <Link to="/"  className="bg-indigo-600 hover:bg-indigo-700 text-white font-bold text-lg py-2 px-4 rounded shadow-md transition duration-300 font-serif">
            🎓 Your Live Mentor
</Link> */}