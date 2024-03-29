export interface IUser {
  username: string;
  displayName: string;
  token: string;
  image?: string;
  isHost: boolean;
  followerCount: number;
  followingCount: number;
  following: boolean;
}

export interface IUserFormValues {
  email: string;
  password: string;
  displayName?: string;
  username?: string;
}
