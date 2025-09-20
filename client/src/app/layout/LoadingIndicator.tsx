


type Variant = "spinner" | "dots" | "skeleton";

interface Props {
  variant?: Variant;
  size?: "xs" | "sm" | "md" | "lg" | "xl"; // maps to Tailwind sizes
  colorClass?: string; // Tailwind color classes, e.g. "text-indigo-600"
  label?: string | null; // optional accessible label (visible or screen-reader only)
  className?: string;
}

/**
 * Tailwind-based loading indicator with 3 variants:
 * - spinner: SVG circular spinner (animate-spin)
 * - dots: 3 bouncing/pulsing dots
 * - skeleton: a rectangular shimmer/pulse bar (useful as a placeholder)
 */
export default function LoadingIndicator({
  variant = "spinner",
  size = "md",
  colorClass = "text-indigo-600",
  label = "Loading",
  className = "",
}: Props) {
  const sizeMap: Record<"xs" | "sm" | "md" | "lg" | "xl", string> = {
    xs: "w-4 h-4",
    sm: "w-5 h-5",
    md: "w-6 h-6",
    lg: "w-8 h-8",
    xl: "w-10 h-10",
  };

  const spinnerSize = sizeMap[size];

  if (variant === "dots") {
    return (
      <div
        role="status"
        aria-live="polite"
        className={`inline-flex items-center space-x-2 ${className}`}
      >
        <span className="sr-only">{label}</span>
        <span
          className={`inline-block ${colorClass} ${spinnerSize}`}
          aria-hidden
        >
          <span className="inline-flex items-end space-x-1">
            {/* three dots with staggered animation delays */}
            <span
              className="inline-block rounded-full bg-current opacity-90 animate-pulse"
              style={{ width: "6px", height: "6px", animationDelay: "0s" }}
            />
            <span
              className="inline-block rounded-full bg-current opacity-90 animate-pulse"
              style={{ width: "6px", height: "6px", animationDelay: "0.15s" }}
            />
            <span
              className="inline-block rounded-full bg-current opacity-90 animate-pulse"
              style={{ width: "6px", height: "6px", animationDelay: "0.3s" }}
            />
          </span>
        </span>
      </div>
    );
  }

  if (variant === "skeleton") {
    return (
      <div role="status" aria-live="polite" className={className}>
        <span className="sr-only">{label}</span>
        <div
          className={`rounded-md bg-gray-200 dark:bg-gray-700 animate-pulse ${className}`}
          style={{
            width: "100%",
            height: size === "xs" ? 8 : size === "sm" ? 10 : size === "md" ? 12 : size === "lg" ? 16 : 20,
          }}
        />
      </div>
    );
  }

  // default: spinner (SVG)
  return (
    <div role="status" aria-live="polite" className={`inline-flex items-center ${className}`}>
      <span className="sr-only">{label}</span>
      <svg
        className={`${spinnerSize} ${colorClass} animate-spin`}
        viewBox="0 0 50 50"
        aria-hidden="true"
        fill="none"
      >
        <circle
          className="opacity-25"
          cx="25"
          cy="25"
          r="20"
          stroke="currentColor"
          strokeWidth="5"
        />
        <path
          className="opacity-75"
          fill="currentColor"
          d="M14 25a11 11 0 0111-11v5a6 6 0 00-6 6h-5z"
        />
      </svg>
    </div>
  );
}
