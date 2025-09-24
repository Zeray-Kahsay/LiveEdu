import { apiSlice } from "../../app/api/apiSlice";
import type { CourseDto } from "../../app/types/course/courseDto";

export const courseApi = apiSlice.injectEndpoints({
    endpoints: (builder) => ({
        getCourses: builder.query<CourseDto[], void>({
            query: () => "/courses",
            providesTags: ["Courses"],
        }),
        getCourseById: builder.query<CourseDto, number>({
            query: (id) => `/courses/${id}`,
            providesTags: (_result, _error, id) => [{type: "Courses", id}], 
        }),
        enrollCourse: builder.mutation<void, number>({
            query: (courseId) => ({
                url: `/courses/${courseId}/enroll`,
                method: "POST"
            }),
        invalidatesTags: ["Courses"],
        })
    }),// endpoints 
});

export const {
    useGetCoursesQuery, 
    useGetCourseByIdQuery,
    useEnrollCourseMutation
} = courseApi;