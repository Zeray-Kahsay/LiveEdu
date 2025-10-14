import {
  fetchBaseQuery,
  type BaseQueryApi,
  type FetchArgs,
  type FetchBaseQueryError,
} from "@reduxjs/toolkit/query";
import { startLoading, stopLoading } from "../layout/uiSlice";
import { toast } from "react-toastify";
import { setCredentials, logout } from "../../features/auth/authSlice";
import type { RootState } from "../store/store";
import type { AuthResponse } from "../types/account/authResponse";

const rawBaseQuery = fetchBaseQuery({
  baseUrl: import.meta.env.VITE_API_URL,
  prepareHeaders: (headers, { getState }) => {
    const token = (getState() as RootState).auth.accessToken;
    if (token) headers.set("Authorization", `Bearer ${token}`);
    return headers;
  },
});

 const sleep = () => new Promise((resolve) => setTimeout(resolve, 1000));


// unified base query
export const baseQueryWithReauthAndErrorHandling = async (
  args: string | FetchArgs,
  api: BaseQueryApi,
  extraOptions: object
) => {
  api.dispatch(startLoading());

  let result = await rawBaseQuery(args, api, extraOptions);

  // check for 401 (expired token)
  if (result.error && result.error.status === 401) {
    const refreshToken = (api.getState() as RootState).auth.refreshToken;

    if (refreshToken) {
      const refreshResult = await rawBaseQuery(
        {
          url: "/account/refresh-token",
          method: "POST",
          body: { refreshToken },
        },
        api,
        extraOptions
      );

      if (refreshResult.data) {
        const authData = refreshResult.data as AuthResponse;
        api.dispatch(setCredentials(authData));

        await sleep();

        // retry original request
        result = await rawBaseQuery(args, api, extraOptions);
      } else {
        api.dispatch(logout());
        window.location.href = "/login";
      }
    } else {
      api.dispatch(logout());
      window.location.href = "/login";
    }
  }

  // error handling
  if (result.error) {
    const { status, data } = result.error as FetchBaseQueryError;
    switch (status) {
      case 400:
        toast.error((data as any) || "Bad Request");
        break;
      case 401:
        toast.error((data as any) || "Unauthorized");
        break;
      case 403:
        toast.error("Forbidden");
        break;
      case 500:
        toast.error("Internal server error");
        break;
      default:
        break;
    }
  }

  api.dispatch(stopLoading());
  return result;
};

