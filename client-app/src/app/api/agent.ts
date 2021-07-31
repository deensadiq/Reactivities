import axios, { AxiosError, AxiosResponse } from "axios";
import { IActivity } from "../models/activity";
import { history } from "../..";
import { toast } from "react-toastify";
import { IUser, IUserFormValues } from "../models/user";
import { IPhoto, IProfile, IProfileFormValues } from "../models/profile";
import { PaginatedResult } from "../models/pagination";
import IUserActivity from "../models/UserActivity";

axios.defaults.baseURL = "http://localhost:5000/api";

axios.interceptors.request.use(
  (config) => {
    const token = window.localStorage.getItem("jwt");
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
  },
  (error: AxiosError) => {
    return Promise.reject(error as AxiosError);
  }
);

axios.interceptors.response.use(
  async (response) => {
    await sleep(10000);

    const pagination = response.headers["pagination"];

    if (pagination) {
      response.data = new PaginatedResult(
        response.data,
        JSON.parse(pagination)
      );

      return response as AxiosResponse<PaginatedResult<any>>;
    }

    return response;
  },
  (error) => {
    if (error.message === "Network Error" && !error.reponse) {
      toast.error("NETWORK ERROR - Make sure the API is running!");
    }

    const { status, data, config } = error.response;

    if (status === 404) {
      history.push("/notfound");
    }

    if (
      status === 400 &&
      config.method === "get" &&
      data.errors.hasOwnProperty("id")
    ) {
      history.push("/notfound");
    }

    if (status === 500) {
      toast.error("Server error - check the terminal for more info!");
    }

    throw error.response;
  }
);

const responseBody = (response: AxiosResponse) => response.data;

const sleep = (ms: number) => (response: AxiosResponse) =>
  new Promise<AxiosResponse>((resolve) =>
    setTimeout(() => resolve(response), ms)
  );

const requests = {
  get: (url: string) => axios.get(url).then(sleep(1000)).then(responseBody),
  post: (url: string, body: {}) =>
    axios.post(url, body).then(sleep(1000)).then(responseBody),
  put: (url: string, body: {}) =>
    axios.put(url, body).then(sleep(1000)).then(responseBody),
  delete: (url: string) =>
    axios.delete(url).then(sleep(1000)).then(responseBody),
  postForm: (url: string, file: Blob) => {
    let formData = new FormData();
    formData.append("File", file);
    return axios
      .post(url, formData, {
        headers: { "Content-type": "multipart/form-data" },
      })
      .then(sleep(1000))
      .then(responseBody);
  },
};

const Activities = {
  list: (params: URLSearchParams): Promise<PaginatedResult<IActivity[]>> =>
    axios.get("/activities", { params }).then(sleep(1000)).then(responseBody),
  details: (id: string): Promise<IActivity> =>
    requests.get(`/activities/${id}`),
  create: (activity: IActivity) => requests.post("/activities", activity),
  update: (activity: IActivity) =>
    requests.put(`/activities/${activity.id}`, activity),
  delete: (id: string) => requests.delete(`/activities/${id}`),
  attend: (id: string) => requests.post(`/activities/${id}/attend`, {}),
  unattend: (id: string) => requests.delete(`/activities/${id}/attend`),
};

const User = {
  current: (): Promise<IUser> => requests.get("/user"),
  login: (user: IUserFormValues): Promise<IUser> =>
    requests.post(`/user/login`, user),
  register: (user: IUserFormValues): Promise<IUser> =>
    requests.post(`/user/register`, user),
};

const Profiles = {
  detail: (username: string): Promise<IProfile> =>
    requests.get(`/profiles/${username}`),
  update: (profile: IProfileFormValues) => requests.put(`/profiles`, profile),
  uploadPhoto: (photo: Blob): Promise<IPhoto> =>
    requests.postForm(`/photos`, photo),
  deletePhoto: (id: string) => requests.delete(`/photos/${id}`),
  setMainPhoto: (id: string) => requests.post(`/photos/${id}/setmain`, {}),
  activityList: (
    username: string,
    params: URLSearchParams
  ): Promise<IUserActivity[]> =>
    axios
      .get(`/profiles/${username}/activities`, { params })
      .then(sleep(1000))
      .then(responseBody),
};

export default {
  Activities,
  User,
  Profiles,
};
