import type { ReactNode } from "react";
import { Link } from "react-router-dom";

type EmptyStateProps = {
  icon?: ReactNode;
  title: string;
  description?: string;
  actionLabel?: string;
  actionLink?: string;
};

const EmptyState = ({ icon, title, description, actionLabel, actionLink }: EmptyStateProps) => (
  <div className="flex flex-col items-center justify-center py-12 text-center space-y-4">
    {icon && <div className="text-blue-500 w-12 h-12">{icon}</div>}
    <h2 className="text-lg font-semibold">{title}</h2>
    {description && <p className="text-gray-500">{description}</p>}
    {actionLabel && actionLink && (
      <Link
        to={actionLink}
        className="px-4 py-2 rounded-xl bg-blue-600 text-white hover:bg-blue-700 transition"
      >
        {actionLabel}
      </Link>
    )}
  </div>
);

export default EmptyState;
