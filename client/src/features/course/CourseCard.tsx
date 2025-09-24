import { Button } from "../../app/layout/ui/Button";
import { useEnrollCourseMutation } from "./EnrollmentApi";

type CourseCardProps = {
    id: number;
    title:string;
    description: string;
    subject: string;
    gradeLevel: string;
    teacherName: string;
    studentId: number; // from authState
}


const CourseCard = ({
    id, 
    title,
    description,
    subject,
    gradeLevel,
    teacherName,
    studentId
} : CourseCardProps) => {
    const [enrollCourse, {isLoading, isSuccess, isError, error}] = useEnrollCourseMutation();


    const handleEnroll = async () => {
        try {
            await enrollCourse({ courseId: id, studentId}).unwrap();
        } catch (error) {
            console.log("Enrollment failed", error);
        }
    }



  return (
    <div className="rounded-2xl shadow-md p-4 bg-white space-y-3">
      <h2 className="text-lg font-semibold">{title}</h2>
      <p className="text-gray-600">{description}</p>
      <p className="text-sm text-gray-500">
        {subject} • {gradeLevel}
      </p>
      <p className="text-sm text-gray-500">👨‍🏫 {teacherName}</p>

      <Button
        className="w-full"
        onClick={handleEnroll}
        disabled={isLoading}
      >
        {isLoading ? "Enrolling..." : isSuccess ? "Enrolled ✅" : "Enroll"}
      </Button>

      {isError && (
        <p className="text-red-500 text-sm mt-2">
          {(error as any)?.data?.errors?.[0] || "Enrollment failed"}
        </p>
      )}
    </div>
  )
}

export default CourseCard
