import type { UserDto } from "./userDto";

export interface AuthResponse {
  user: UserDto | null;
  accessToken: string;
  refreshToken: string;
}