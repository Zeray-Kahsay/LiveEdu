import { useNavigate, useOutletContext } from "react-router-dom";
import LoadingIndicator from "../../app/layout/LoadingIndicator";
import { useGetStudentEnrollmentsQuery } from "../course/EnrollmentApi";
import EmptyState from "../../app/layout/ui/EmptyState";
import { BookOpen } from "lucide-react";
import type { dashboardContext } from "../../app/types/dashboard/DashboardContext";


const Dashboard = () => {
    const {studentId} = useOutletContext<dashboardContext>();
    const {data: enrollments, isLoading} = useGetStudentEnrollmentsQuery(studentId);
    const navigate = useNavigate();

    if (isLoading) return <LoadingIndicator variant="spinner" size="lg" colorClass="white-text" fullPage /> 
    if (!enrollments || enrollments.length === 0){
      return (
        <EmptyState
         icon={<BookOpen className="w-12 h-12" />}
         title="You're yet to enroll in a course"
         description="Browse our catalog and start learning today."
         actionLabel="Go to Catalog"
         actionLink="/catalog"
         />
      )
    }
  return (
  <div className="space-y-4 p-4">
      <h1 className="text-2xl font-semibold">📚 My Enrolled Courses</h1>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {enrollments.map((e) => (
          <div
            key={e.enrollmentId}
            className="rounded-2xl shadow-md p-4 bg-white cursor-pointer hover:shadow-lg transition"
            onClick={() => navigate(`/dashboard/course/${e.courseId}`)}
          >
            <h2 className="text-lg font-semibold">{e.courseTitle}</h2>
            <p className="text-sm text-gray-500">
              {e.subject} • {e.gradeLevel}
            </p>
            <p className="text-sm text-gray-500">👨‍🏫 {e.teacherName}</p>
            <p className="text-xs text-gray-400">Enrolled: {new Date(e.enrolledAt).toLocaleDateString()}</p>
            <p className="text-xs text-green-600 font-medium">{e.status}</p>
          </div>
        ))}
      </div>
    </div>
  )
}

export default Dashboard
