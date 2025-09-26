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
        getCoursesByFilter: builder.query<CourseDto[], {grade?: string; subject?: string}>({
            query: ({grade, subject}) => {
                const params = new URLSearchParams();
                if (grade) params.append("grade", grade);
                if (subject) params.append("subject", subject);
                return `/courses/filter?${params.toString()}`
            },
            providesTags: ["Courses"]
        })
      
    }),// endpoints 
});

export const {
    useGetCoursesQuery, 
    useGetCourseByIdQuery,
    useGetCoursesByFilterQuery,
    //useEnrollCourseMutation
} = courseApi;