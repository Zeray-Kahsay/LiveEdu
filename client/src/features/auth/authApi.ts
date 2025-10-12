import { apiSlice } from "../../app/api/apiSlice";
import type { AuthResponse } from "../../app/types/account/authResponse";
import type { LoginDto } from "../../app/types/account/loginDto";
import type { RegisterDto } from "../../app/types/account/registerDto";
import type { UserDto } from "../../app/types/account/userDto";
import { setCredentials } from "./authSlice";

export const authApi = apiSlice.injectEndpoints({
  endpoints: (builder) => ({
    registerUser: builder.mutation<UserDto, RegisterDto>({
      query: (user) => ({
        url: "/accounts/registerUser",
        method: "POST",
        body: user,
      }),
    }),
    loginUser: builder.mutation<AuthResponse, LoginDto>({
      query: (creds) => ({
        url: "/accounts/loginUser",
        method: "POST",
        body: creds,
      }),
      async onQueryStarted(_, {dispatch, queryFulfilled}){
        try {
            const {data} = await queryFulfilled;
            // save to Redux
            dispatch(setCredentials(data));
            // Persist to localStorage
            localStorage.setItem("auth", JSON.stringify(data));
        } catch (error) {
          console.log("Login failed", error);
        }
      }
    }),
    refreshToken: builder.mutation<AuthResponse, { refreshToken: string; deviceId: string }>({
      query: ({ refreshToken, deviceId }) => ({
        url: "/accounts/refreshToken",
        method: "POST",
        body: { refreshToken, deviceId },
      }),
    }),
  }),
  overrideExisting: true,
});

export const { useRegisterUserMutation, useLoginUserMutation, useRefreshTokenMutation } = authApi;
