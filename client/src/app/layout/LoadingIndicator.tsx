interface Props {
  variant?: "spinner" | "dots";
  size?: "sm" | "md" | "lg";
  colorClass?: string;
  fullPage?: boolean; 
}

const LoadingIndicator = ({ variant = "spinner", size = "md", colorClass = "text-indigo-500", fullPage = false }: Props) => {
  const sizeClass = size === "lg" ? "w-12 h-12" : size === "md" ? "w-8 h-8" : "w-4 h-4";

  return (
    <div className={fullPage ? "flex items-center justify-center min-h-screen w-full" : ""}>
      {variant === "spinner" ? (
        <div className={`animate-spin rounded-full border-4 border-t-transparent ${sizeClass} ${colorClass}`} />
      ) : (
        <div className="flex space-x-2">
          <span className={`animate-bounce ${sizeClass} ${colorClass}`}>•</span>
          <span className={`animate-bounce ${sizeClass} ${colorClass} delay-150`}>•</span>
          <span className={`animate-bounce ${sizeClass} ${colorClass} delay-300`}>•</span>
        </div>
      )}
    </div>
  );
};

export default LoadingIndicator;



// type Variant = "spinner" | "dots" | "skeleton";

// interface Props {
//   variant?: Variant;
//   size?: "xs" | "sm" | "md" | "lg" | "xl"; // maps to Tailwind sizes
//   colorClass?: string; // Tailwind color classes, e.g. "text-indigo-600"
//   label?: string | null; // optional accessible label (screen-reader only)
//   className?: string;
//   fullPage?: boolean;
// }

// export default function LoadingIndicator({
//   variant = "spinner",
//   size = "md",
//   colorClass = "text-indigo-600",
//   label = "Loading",
//   className = "",
//   fullPage = false
// }: Props) {
//   const sizeMap: Record<typeof size, string> = {
//     xs: "w-4 h-4",
//     sm: "w-5 h-5",
//     md: "w-6 h-6",
//     lg: "w-8 h-8",
//     xl: "w-10 h-10",
//   };

//   const spinnerSize = sizeMap[size];

//   if (variant === "dots") {
//     return (
//       <div
//         role="status"
//         aria-live="polite"
//         className={`inline-flex items-center space-x-1 ${className}`}
//       >
//         <span className="sr-only">{label}</span>
//         <span className={`flex space-x-1 ${colorClass}`}>
//           <span className="bounce-dot"></span>
//           <span className="bounce-dot"></span>
//           <span className="bounce-dot"></span>
//         </span>
//       </div>
//     );
//   }

//   if (variant === "skeleton") {
//     return (
//       <div role="status" aria-live="polite" className={className}>
//         <span className="sr-only">{label}</span>
//         <div
//           className={`rounded-md bg-gray-200 dark:bg-gray-700 animate-pulse`}
//           style={{
//             width: "100%",
//             height:
//               size === "xs"
//                 ? 8
//                 : size === "sm"
//                 ? 10
//                 : size === "md"
//                 ? 12
//                 : size === "lg"
//                 ? 16
//                 : 20,
//           }}
//         />
//       </div>
//     );
//   }

//   // default spinner variant (SVG)
//   return (
//     <div role="status" aria-live="polite" className={`inline-flex items-center ${className}`}>
//       <span className="sr-only">{label}</span>
//       <svg
//         className={`${spinnerSize} ${colorClass} animate-spin`}
//         viewBox="0 0 50 50"
//         aria-hidden="true"
//         fill="none"
//       >
//         <circle
//           className="opacity-25"
//           cx="25"
//           cy="25"
//           r="20"
//           stroke="currentColor"
//           strokeWidth="5"
//         />
//         <path
//           className="opacity-75"
//           fill="currentColor"
//           d="M14 25a11 11 0 0111-11v5a6 6 0 00-6 6h-5z"
//         />
//       </svg>
//     </div>
//   );
// }
