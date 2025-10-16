import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { useState } from "react";
import RichTextEditor from "../../app/layout/ui/RichTextEditor";
import { useCreateCourseMutation } from "./courseApi";

export const courseSchema = z.object({
  title: z.string().min(3),
  description: z.string().min(10),
  price: z.number().min(0),
  imageUrl: z.string().url(),
  startDate: z.string().refine(date => !isNaN(Date.parse(date)), { message: "Invalid date" }),
  endDate: z.string().refine(date => !isNaN(Date.parse(date)), { message: "Invalid date" }),
  maxStudents: z.number().min(1),
  gradeLevel: z.enum(["Grade1", "Grade2", "Grade3", "Grade4", "Grade5", "Grade6", "Grade7", "Grade8", "Grade9", "Grade10", "Grade11", "Grade12"]),
  subject: z.string().min(2),
});



type CreateCourseInput = z.infer<typeof courseSchema>;

const CreateCourseForm = () => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: zodResolver(courseSchema),
    
  });

  const [createCourse, { isLoading, isSuccess, isError, error }] = useCreateCourseMutation();
  const [description, setDescription] = useState("");

  const onSubmit = async (data: CreateCourseInput) => {
     const payload = { ...data, description };
     console.log("Form submitted with data:", payload);
    // try {
    //   await createCourse(payload).unwrap();
    //   alert("Course created!");
    //   console.log("Course created successfully", payload);
    //   console.log("payload:", payload); 
    // } catch (err) {
    //   console.error("Course creation failed", err);
    // }
  };

  return (
    <form
      onSubmit={handleSubmit(onSubmit)}
      className="max-w-2xl mx-auto bg-white shadow-lg rounded-xl p-8 space-y-6"
    >
      <h2 className="text-2xl font-bold text-indigo-700">📚 Create a New Course</h2>

      {/* Title */}
      <div>
        <label className="block text-sm font-medium text-gray-700">Course Title</label>
        <input
          {...register("title")}
          className="mt-1 w-full border border-gray-300 rounded-md px-4 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
          placeholder="e.g. Algebra Basics"
        />
        {errors.title && <p className="text-red-500 text-sm mt-1">{errors.title.message}</p>}
      </div>

      {/* Rich Text Description */}
      <div>
        <label className="block text-sm font-medium text-gray-700 mb-1">Description</label>
        <RichTextEditor onChange={setDescription} />
        {description.length < 10 && (
          <p className="text-red-500 text-sm mt-1">Description must be at least 10 characters</p>
        )}
      </div>

      {/* Price and Max Students */}
      <div className="grid grid-cols-2 gap-4">
        <div>
          <label className="block text-sm font-medium text-gray-700">Price (USD)</label>
          <input
            type="number"
            {...register("price", { valueAsNumber: true })}
            className="mt-1 w-full border border-gray-300 rounded-md px-4 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
            placeholder="e.g. 49.99"
          />
          {errors.price && <p className="text-red-500 text-sm mt-1">{errors.price.message}</p>}
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700">Max Students</label>
          <input
            type="number"
            {...register("maxStudents", { valueAsNumber: true })}
            className="mt-1 w-full border border-gray-300 rounded-md px-4 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
            placeholder="e.g. 30"
          />
          {errors.maxStudents && <p className="text-red-500 text-sm mt-1">{errors.maxStudents.message}</p>}
        </div>
      </div>

      {/* Dates */}
      <div className="grid grid-cols-2 gap-4">
        <div>
          <label className="block text-sm font-medium text-gray-700">Start Date</label>
          <input
            type="date"
            {...register("startDate")}
            className="mt-1 w-full border border-gray-300 rounded-md px-4 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
          {errors.startDate && <p className="text-red-500 text-sm mt-1">{errors.startDate.message}</p>}
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700">End Date</label>
          <input
            type="date"
            {...register("endDate")}
            className="mt-1 w-full border border-gray-300 rounded-md px-4 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
          {errors.endDate && <p className="text-red-500 text-sm mt-1">{errors.endDate.message}</p>}
        </div>
      </div>

      {/* Grade Level & Subject */}
      <div className="grid grid-cols-2 gap-4">
        <div>
          <label className="block text-sm font-medium text-gray-700">Grade Level</label>
          <select
            {...register("gradeLevel")}
            className="mt-1 w-full border border-gray-300 rounded-md px-4 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
          >
            <option value="">Select Grade</option>
            {["Grade1", "Grade2", "Grade3", "Grade4", "Grade5", "Grade6", "Grade7", "Grade8", "Grade9", "Grade10", "Grade11", "Grade12"].map(g => (
              <option key={g} value={g}>{g}</option>
            ))}
          </select>
          {errors.gradeLevel && <p className="text-red-500 text-sm mt-1">{errors.gradeLevel.message}</p>}
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700">Subject</label>
          <input
            {...register("subject")}
            className="mt-1 w-full border border-gray-300 rounded-md px-4 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
            placeholder="e.g. Math"
          />
          {errors.subject && <p className="text-red-500 text-sm mt-1">{errors.subject.message}</p>}
        </div>
      </div>

      {/* Image URL */}
      <div>
        <label className="block text-sm font-medium text-gray-700">Image URL</label>
        <input
          {...register("imageUrl")}
          className="mt-1 w-full border border-gray-300 rounded-md px-4 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
          placeholder="https://example.com/image.jpg"
        />
        {errors.imageUrl && <p className="text-red-500 text-sm mt-1">{errors.imageUrl.message}</p>}
      </div>

      {/* Submit Button */}
      <button
        type="submit"
        disabled={isLoading}
        className="w-full bg-indigo-600 text-white py-2 px-4 rounded-md hover:bg-indigo-700 transition disabled:opacity-50"
      >
        {isLoading ? "Creating..." : "Create Course"}
      </button>

      {/* Feedback */}
      {isError && <p className="text-red-500 text-sm mt-2">{(error as any)?.data?.errors?.[0]}</p>}
      {isSuccess && <p className="text-green-600 text-sm mt-2">Course created successfully ✅</p>}
    </form>
  );
};

export default CreateCourseForm;






