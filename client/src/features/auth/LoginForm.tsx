import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { FiMail, FiLock } from "react-icons/fi";
import { useLoginUserMutation } from "./authApi";
import { toast } from "react-toastify";
import LoadingIndicator from "../../app/layout/LoadingIndicator";
import { useNavigate } from "react-router-dom";
import { useState } from "react";

const loginSchema = z.object({
  email: z.string().email("Invalid email"),
  password: z.string().min(6, "Password must be at least 6 characters"),
  deviceId: z.string(),
});

type LoginInput = z.infer<typeof loginSchema>;

export default function LoginForm() {
  const { register, handleSubmit, reset, formState: { errors, isSubmitting } } = useForm<LoginInput>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      deviceId: navigator.userAgent, // simple device ID for web
    }
  });

  const [loginUser] = useLoginUserMutation();
  const navigate = useNavigate();

  const onSubmit = async (data: LoginInput) => {
  try {
    const deviceId = localStorage.getItem("deviceId") ?? crypto.randomUUID();
    localStorage.setItem("deviceId", deviceId);

    const result = await loginUser({ ...data, deviceId }).unwrap();
    reset();
    navigate("/dashboard");

    toast.success(`Welcome back, ${result.user.firstName}!`);
  } catch (err: any) {
    if (err.data?.errors) {
      toast.error(err.data.errors.join(", "));
    } else {
      toast.error("Login failed. Please try again");
    }
  }
};

  return (
    <div className="max-w-md mx-auto mt-6 bg-white p-8 rounded-2xl shadow-lg">
      <h2 className="text-2xl font-bold mb-6 text-indigo-700 text-center">Login</h2>
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        {/* Email */}
        <div className="relative">
          <FiMail className="absolute top-3 left-3 text-indigo-400" />
          <input
            type="email"
            placeholder="Email"
            {...register("email")}
            className="w-full pl-10 pr-3 py-2 rounded-xl border border-gray-300 focus:border-indigo-400 focus:ring-1 focus:ring-indigo-400"
          />
          {errors.email && <p className="text-red-500 text-sm mt-1">{errors.email.message}</p>}
        </div>

        {/* Password */}
        <div className="relative">
          <FiLock className="absolute top-3 left-3 text-indigo-400" />
          <input
            type="password"
            placeholder="Password"
            {...register("password")}
            className="w-full pl-10 pr-3 py-2 rounded-xl border border-gray-300 focus:border-indigo-400 focus:ring-1 focus:ring-indigo-400"
          />
          {errors.password && <p className="text-red-500 text-sm mt-1">{errors.password.message}</p>}
        </div>

        {/* Submit button */}
        <button
         disabled={isSubmitting}
          type="submit"
          className="w-full py-2 rounded-xl bg-indigo-500 text-white font-bold hover:bg-indigo-600 transition flex items-center justify-center"
        >
          {isSubmitting ? (
            <LoadingIndicator  variant="dots" size="sm" colorClass="text-white" className="justify-center"  />
          ): (
           "Login"
          )}
        </button>
        <div>

        </div>
      </form>
    </div>
  );
}



// import { zodResolver } from "@hookform/resolvers/zod";
// import { useForm } from "react-hook-form";
// import { z } from "zod";
// import { FaEnvelope, FaLock } from "react-icons/fa";
// import { toast } from "react-toastify";
// import { useLoginUserMutation } from "./authApi";

// const loginSchema = z.object({
//   email: z.string().email("Invalid email"),
//   password: z.string().min(6, "Password must be at least 6 characters"),
// });

// type LoginFormInputs = z.infer<typeof loginSchema>;

// export default function LoginForm() {
//   const { register, handleSubmit, formState: { errors } } = useForm<LoginFormInputs>({
//     resolver: zodResolver(loginSchema),
//   });

//   const [loginUser] = useLoginUserMutation();

//   const onSubmit = async (data: LoginFormInputs) => {
//     try {
//       const deviceId = localStorage.getItem("deviceId") ?? crypto.randomUUID();
//       localStorage.setItem("deviceId", deviceId);

//       const result = await loginUser({ ...data, deviceId }).unwrap();
//       console.log(result);
//       toast.success(`Welcome back, ${result.firstName}!`);
//     } catch (err: any) {
//       if (err.data?.errors){
//         toast.error(err?.data?.errors?.join(", "));
//       } else {
//         toast.error("Login failed. Please try again")
//       }
//     }
//   };

//   return (
//     <form onSubmit={handleSubmit(onSubmit)} className="max-w-md mx-auto p-6 mt-16 bg-white rounded-xl shadow-md space-y-4">
//    <h2 className="text-3xl font-bold text-center text-green-700 mr-16">🚀 Login</h2>

//       <div className="relative">
//         <FaEnvelope className="absolute top-3 left-3 text-indigo-500" />
//         <input
//           type="email"
//           placeholder="Email"
//           {...register("email")}
//           className={`pl-10 pr-4 py-2 w-full border rounded-full focus:outline-none focus:ring-2 focus:ring-indigo-500 ${
//             errors.email ? "border-red-500" : "border-gray-300"
//           }`}
//         />
//         {errors.email && <p className="text-red-500 text-sm mt-1">{errors.email.message}</p>}
//       </div>

//       <div className="relative">
//         <FaLock className="absolute top-3 left-3 text-indigo-500" />
//         <input
//           type="password"
//           placeholder="Password"
//           {...register("password")}
//           className={`pl-10 pr-4 py-2 w-full border rounded-full focus:outline-none focus:ring-2 focus:ring-indigo-500 ${
//             errors.password ? "border-red-500" : "border-gray-300"
//           }`}
//         />
//         {errors.password && <p className="text-red-500 text-sm mt-1">{errors.password.message}</p>}
//       </div>

//       <button
//         type="submit"
//         className="w-full bg-indigo-600 hover:bg-indigo-700 text-white font-semibold py-2 rounded-full transition-all"
//       >
//         Login
//       </button>
//     </form>
//   );
// }



 
