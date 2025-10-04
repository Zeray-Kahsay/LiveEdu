interface PaginationProps {
  onNext: () => void;
  onPrevious: () => void;
  canGoNext: boolean;
  canGoBack: boolean;
}

const Pagination = ({
  onNext,
  onPrevious,
  canGoNext,
  canGoBack,
}: PaginationProps) => {
  return (
    <div className="flex justify-between mt-6">
      <button
        onClick={onPrevious}
        disabled={!canGoBack}
        className="px-4 py-2 rounded bg-gray-200 disabled:opacity-50"
      >
        ← Previous
      </button>

      <button
        onClick={onNext}
        disabled={!canGoNext}
        className="px-4 py-2 rounded bg-blue-500 text-white disabled:opacity-50"
      >
        Next →
      </button>
    </div>
  );
};

export default Pagination;
