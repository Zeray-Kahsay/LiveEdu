interface CourseFiltersProps {
  filters: { gradeLevel: string; subject: string };
  searchInput: string;
  onFilterChange: (key: string, value: string) => void;
  onSearchChange: (value: string) => void;
}

const CourseFilters = ({
  filters,
  searchInput,
  onFilterChange,
  onSearchChange,
}: CourseFiltersProps) => {
  return (
    <div className="sticky top-0 z-10 bg-white shadow-sm p-4 rounded-md mb-4">
      <div className="flex flex-wrap gap-4 items-center">
        {/* Search */}
        <input
          type="text"
          placeholder="Search courses..."
          value={searchInput}
          onChange={(e) => onSearchChange(e.target.value)}
            className="border rounded-xl p-2 shadow-sm focus:ring-2 focus:ring-indigo-400 focus:font-serif  flex-1 min-w-[200px]"
        />

        {/* Grade filter */}
        <select
          value={filters.gradeLevel}
          onChange={(e) => onFilterChange("gradeLevel", e.target.value)}
            className="border rounded-xl p-2 shadow-sm focus:ring-2 focus:ring-indigo-400 focus:font-serif"
        >
          <option value="">All Grades</option>
          <option value="Grade1">Grade 1</option>
          <option value="Grade6">Grade 6</option>
          <option value="Grade8">Grade 8</option>
          <option value="Grade10">Grade 10</option>
        </select>

        {/* Subject filter */}
        <select
          value={filters.subject}
          onChange={(e) => onFilterChange("subject", e.target.value)}
           className="border rounded-xl p-2 shadow-sm focus:ring-2 focus:ring-indigo-400 focus:font-serif"
        >
          <option value="">All Subjects</option>
          <option value="Math">Math</option>
          <option value="Science">Science</option>
          <option value="History">History</option>
        </select>
      </div>
    </div>
  );
};

export default CourseFilters;
