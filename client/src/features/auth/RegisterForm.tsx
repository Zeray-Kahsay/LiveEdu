import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { FiMail, FiLock, FiUser, FiBook } from "react-icons/fi";
import { toast } from "react-toastify";
import { useRegisterUserMutation } from "./authApi";
import LoadingIndicator from "../../app/layout/LoadingIndicator";

const registerSchema = z.object({
  firstName: z.string().min(2, "First name is required"),
  lastName: z.string().min(2, "Last name is required"),
  email: z.string().email("Invalid email"),
  password: z.string().min(6, "Password must be at least 6 characters"),
  confirmPassword: z.string().min(6),
  schoolName: z.string().min(2, "School name is required"),
  role: z.enum(["Student", "Teacher", "Parent", "Admin"]),
}).refine((data) => data.password === data.confirmPassword, {
  message: "Passwords do not match",
  path: ["confirmPassword"],
});

type RegisterInput = z.infer<typeof registerSchema>;

export default function RegisterForm() {
  const { register, handleSubmit, reset, formState: { errors, isSubmitting } } = useForm<RegisterInput>({
    resolver: zodResolver(registerSchema),
  });

  const [registerUser] = useRegisterUserMutation();

  const onSubmit = async (data: RegisterInput) => {
    try {
      const result = await registerUser(data).unwrap();
      toast.success(`Welcome, ${result.firstName}!`);
      reset();
    } catch (err: any) {
      if (err.data?.errors) {
        toast.error(err.data.errors.join(", "));
      } else {
        toast.error("Registration failed. Try again.");
      }
    }
  };

  return (
    <div className="max-w-md mx-auto mt-6 bg-white p-8 rounded-2xl shadow-lg">
      <h2 className="text-2xl font-bold mb-6 text-indigo-700 text-center">Register</h2>
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        {/* First Name */}
        <div className="relative">
          <FiUser className="absolute top-3 left-3 text-indigo-400" />
          <input
            type="text"
            placeholder="First Name"
            {...register("firstName")}
            className="w-full pl-10 pr-3 py-2 rounded-xl border border-gray-300 focus:border-indigo-400 focus:ring-1 focus:ring-indigo-400"
          />
          {errors.firstName && <p className="text-red-500 text-sm mt-1">{errors.firstName.message}</p>}
        </div>

        {/* Last Name */}
        <div className="relative">
          <FiUser className="absolute top-3 left-3 text-indigo-400" />
          <input
            type="text"
            placeholder="Last Name"
            {...register("lastName")}
            className="w-full pl-10 pr-3 py-2 rounded-xl border border-gray-300 focus:border-indigo-400 focus:ring-1 focus:ring-indigo-400"
          />
          {errors.lastName && <p className="text-red-500 text-sm mt-1">{errors.lastName.message}</p>}
        </div>

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

        {/* School Name */}
        <div className="relative">
          <FiBook className="absolute top-3 left-3 text-indigo-400" />
          <input
            type="text"
            placeholder="School Name"
            {...register("schoolName")}
            className="w-full pl-10 pr-3 py-2 rounded-xl border border-gray-300 focus:border-indigo-400 focus:ring-1 focus:ring-indigo-400"
          />
          {errors.schoolName && <p className="text-red-500 text-sm mt-1">{errors.schoolName.message}</p>}
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

        {/* Confirm Password */}
        <div className="relative">
          <FiLock className="absolute top-3 left-3 text-indigo-400" />
          <input
            type="password"
            placeholder="Confirm Password"
            {...register("confirmPassword")}
            className="w-full pl-10 pr-3 py-2 rounded-xl border border-gray-300 focus:border-indigo-400 focus:ring-1 focus:ring-indigo-400"
          />
          {errors.confirmPassword && <p className="text-red-500 text-sm mt-1">{errors.confirmPassword.message}</p>}
        </div>

        {/* Role select */}
        <div className="relative">
          <select
            {...register("role")}
            className="w-full pl-3 pr-3 py-2 rounded-xl border border-gray-300 focus:border-indigo-400 focus:ring-1 focus:ring-indigo-400 appearance-none"
          >
            <option value="">Select Role</option>
            <option value="Student">Student</option>
            <option value="Teacher">Teacher</option>
            <option value="Parent">Parent</option>
            <option value="Admin">Admin</option>
          </select>
          {errors.role && <p className="text-red-500 text-sm mt-1">{errors.role.message}</p>}
        </div>

        {/* Submit button */}
        <button
          type="submit"
          className="w-full py-2 rounded-xl bg-indigo-500 text-white font-bold hover:bg-indigo-600 transition"
        >
          {isSubmitting ? (
            <LoadingIndicator variant="dots" size="sm" colorClass="text-white"/>
          ) : (
            "Register"
          )}
         
        </button>
      </form>
    </div>
  );
}



// import { useForm } from "react-hook-form";
// import { zodResolver } from "@hookform/resolvers/zod";
// import { z } from "zod";
// import { useRegisterUserMutation } from "../../features/auth/authApi";
// import { toast } from "react-toastify";
// import { FaUser, FaEnvelope, FaLock, FaSchool, FaUserTie } from "react-icons/fa";
// import type { JSX } from "react";

// const getDeviceId = () => {
//   let id = localStorage.getItem("deviceId");
//   if (!id) {
//     id = crypto.randomUUID();
//     localStorage.setItem("deviceId", id);
//   }
//   return id;
// };

// // Zod schema
// const registerSchema = z
//   .object({
//     firstName: z.string().min(1, "First name is required"),
//     lastName: z.string().min(1, "Last name is required"),
//     email: z.string().email("Invalid email"),
//     password: z.string().min(6, "Password must be at least 6 characters"),
//     confirmPassword: z.string().min(6, "Confirm your password"),
//     schoolName: z.string().min(1, "School name is required"),
//     role: z.enum(["Student", "Teacher", "Parent"], "Select a role"),
//   })
//   .refine((data) => data.password === data.confirmPassword, {
//     message: "Passwords do not match",
//     path: ["confirmPassword"],
//   });

// type RegisterFormInputs = z.infer<typeof registerSchema>;

// export default function RegisterForm() {
//   const [registerUser, { isLoading }] = useRegisterUserMutation();
//   const { register, handleSubmit, formState: { errors } } = useForm<RegisterFormInputs>({
//     resolver: zodResolver(registerSchema),
//   });

//   const onSubmit = async (data: RegisterFormInputs) => {
//     try {
//       const deviceId = getDeviceId();
//       const result = await registerUser({ ...data, deviceId }).unwrap();
//       toast.success(`Welcome, ${result.firstName}!`);
//     } catch (err: any) {
//       if (err.data?.errors) {
//         toast.error(err.data.errors.join(", "));
//       } else {
//         toast.error("Registration failed. Try again.");
//       }
//     }
//   };

//   // Helper to render an input with icon
//   const InputWithIcon = ({
//     icon,
//     type = "text",
//     placeholder,
//     fieldName,
//   }: { icon: JSX.Element; type?: string; placeholder: string; fieldName: keyof RegisterFormInputs }) => (
//     <div className="relative">
//       <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none text-purple-500">
//         {icon}
//       </div>
//       <input
//         type={type}
//         placeholder={placeholder}
//         {...register(fieldName)}
//         className="w-full pl-10 pr-3 py-2 rounded-full border focus:outline-none focus:ring-2 focus:ring-purple-400"
//       />
//       {errors[fieldName] && (
//         <p className="text-red-600 text-sm mt-1">{errors[fieldName]?.message as string}</p>
//       )}
//     </div>
//   );

//   return (
//     <div className="max-w-md mx-auto mt-10  p-6 bg-yellow-100 rounded-lg shadow-md">
//         <h2 className="text-3xl font-bold text-center text-purple-700 mb-3.5">🎉 Register!</h2>
//       <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
//         <InputWithIcon icon={<FaUser />} placeholder="First Name" fieldName="firstName" />
//         <InputWithIcon icon={<FaUser />} placeholder="Last Name" fieldName="lastName" />
//         <InputWithIcon icon={<FaEnvelope />} placeholder="Email" type="email" fieldName="email" />
//         <InputWithIcon icon={<FaLock />} placeholder="Password" type="password" fieldName="password" />
//         <InputWithIcon icon={<FaLock />} placeholder="Confirm Password" type="password" fieldName="confirmPassword" />
//         <InputWithIcon icon={<FaSchool />} placeholder="School Name" fieldName="schoolName" />
//         <div className="relative">
//           <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none text-purple-500">
//             <FaUserTie />
//           </div>
//           <select
//             {...register("role")}
//             className="w-full pl-10 pr-3 py-2 rounded-full border focus:outline-none focus:ring-2 focus:ring-purple-400"
//           >
//             <option value="">Select role</option>
//             <option value="Student">Student</option>
//             <option value="Teacher">Teacher</option>
//             <option value="Parent">Parent</option>
//           </select>
//           {errors.role && <p className="text-red-600 text-sm mt-1">{errors.role.message}</p>}
//         </div>

//         <button
//           type="submit"
//           disabled={isLoading}
//           className="w-full bg-purple-500 hover:bg-purple-600 text-white font-bold py-2 rounded-full disabled:opacity-50"
//         >
//           {isLoading ? "Registering..." : "Register"}
//         </button>
//       </form>
//     </div>
//   );
// }
