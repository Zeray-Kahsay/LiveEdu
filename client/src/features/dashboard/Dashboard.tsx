import { useOutletContext } from "react-router-dom";
import LoadingIndicator from "../../app/layout/LoadingIndicator";
import { useGetStudentEnrollmentsQuery } from "../course/EnrollmentApi";
import EmptyState from "../../app/layout/ui/EmptyState";
import { BookOpen } from "lucide-react";

type dashboardContext = {
  studentId: number;
}

const Dashboard = () => {
    const {studentId} = useOutletContext<dashboardContext>();
    const {data: enrollments, isLoading} = useGetStudentEnrollmentsQuery(studentId);

    if (isLoading) return <LoadingIndicator variant="spinner" size="lg" colorClass="white-text" /> 
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
    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {enrollments?.map((enrollment) => (
            <div key={enrollment.enrollmentId} className="p-4 bg-white rounded-xl shadow" >
                <h2 className="text-lg font-semibold" > {enrollment.courseTitle} </h2>
                <p>Status:{enrollment.status} </p>
                <p>Student Dashboard for ID: {studentId}</p>
            </div>
        ))}
      
    </div>
  )
}

export default Dashboard
