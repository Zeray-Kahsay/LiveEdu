import { createApi } from "@reduxjs/toolkit/query/react";
import { baseQueryWithReauthAndErrorHandling } from "./baseApi";

export const apiSlice = createApi({
  reducerPath: "api",
  baseQuery: baseQueryWithReauthAndErrorHandling,
  tagTypes: ["User", "Courses", "Enrollments", "Assignments", "Submissions"],
  endpoints: () => ({}),
});
