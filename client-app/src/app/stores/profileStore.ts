import { action, computed, observable, runInAction } from "mobx";
import { toast } from "react-toastify";
import agent from "../api/agent";
import { IPhoto, IProfile, IProfileFormValues } from "../models/profile";
import RootStore from "./rootStore";

export default class ProfileStore {
  rootStore: RootStore;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }

  @observable profile: IProfile | null = null;
  @observable loadingProfile = true;
  @observable loadingUpdateProfile = false;
  @observable uploadingPhoto = false;
  @observable deletingPhoto = false;
  @observable loading = false;

  @computed get IsCurrentUser() {
    if (this.rootStore.userStore.user && this.profile) {
      return this.rootStore.userStore.user.username === this.profile.username;
    } else {
      return false;
    }
  }

  @action loadProfile = async (username: string) => {
    this.loadingProfile = true;
    try {
      const profile = await agent.Profiles.detail(username);
      runInAction("Loading profile.", () => {
        this.profile = profile;
        this.loadingProfile = false;
      });
    } catch (error) {
      console.log(error);
      runInAction("Error loading profile.", () => {
        this.loadingProfile = false;
      });
    }
  };

  @action updateProfile = async (profile: IProfileFormValues) => {
    this.loadingUpdateProfile = true;
    try {
      await agent.Profiles.update(profile);
      runInAction(() => {
        this.rootStore.userStore.user!.displayName = profile.displayName;
        this.profile!.displayName = profile.displayName;
        this.profile!.bio = profile.bio;
        this.loadingUpdateProfile = false;
      })
    } catch(error) {
      console.log(error);
      runInAction(() => {
        this.loadingUpdateProfile = false;
      })
    }
  }

  @action uploadPhoto = async (file: Blob) => {
    this.uploadingPhoto = true;
    try {
      const photo = await agent.Profiles.uploadPhoto(file);
      runInAction(() => {
        this.uploadingPhoto = false;
        if (this.profile) {
          this.profile.photos.push(photo);
          if (photo.isMain && this.rootStore.userStore.user) {
            this.rootStore.userStore.user.image = photo.url;
            this.profile.image = photo.url;
          }
        }
        this.uploadingPhoto = false;
      })
    } catch (error) {
      console.log(error);
      toast.error("Problem uploading photo");
      runInAction(() => {
        this.uploadingPhoto = false;
      })
    }
  }

@action deletePhoto = async (photoId: string) => {
    this.deletingPhoto = true;
    try {
      await agent.Profiles.deletePhoto(photoId);
      runInAction(() => {
        if (this.profile) {
          this.profile!.photos = this.profile!.photos.filter((a) => a.id !== photoId);
          this.deletingPhoto = false;
        }
      })
    } catch(error) {
      console.log(error);
      toast.error("Error deleting photo");
      runInAction(() => {
        this.deletingPhoto = false;
      })
    }
  }

  @action setMainPhoto = async (photo: IPhoto) => {
      this.loading = true;
      try {
        await agent.Profiles.setMainPhoto(photo.id);
        runInAction(() => {
          this.rootStore.userStore.user!.image = photo.url;
          this.profile!.photos.find(a => a.isMain)!.isMain = false;
          this.profile!.photos.find(a => a.id === photo.id)!.isMain = true;
          this.profile!.image = photo.url;
          this.loading = false;
        })
      } catch(error) {
        toast.error("Problem setting main photo.");
        console.log(error);
        runInAction(() => {
          this.loading = false;
        })
      }
  }
}
