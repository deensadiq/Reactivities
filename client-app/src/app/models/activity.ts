export interface IActivity {
  id: string;
  title: string;
  description: string;
  category: string;
  date: Date;
  city: string;
  venue: string;
  isGoing: boolean;
  isHost: boolean;
  attendees: IAttendee[];
  comments: IComment[];
}

export interface IComment {
  activityId: string;
  body: string;
  username: string;
  displayName: string;
  dateCreated: Date;
  image: string;
}

export interface IActivityFormValues extends Partial<IActivity> {
  time?: Date;
}

export class ActivityFormValues implements IActivityFormValues {
  id?: string = undefined;
  title: string = "";
  description: string = "";
  date?: Date = undefined;
  time?: Date = undefined;
  category: string = "";
  city: string = "";
  venue: string = "";

  constructor(init?: IActivityFormValues) {
    if (init && init.date) {
      init.time = init.date;
    }

    Object.assign(this, init);
  }
}

export interface IAttendee {
  username: string;
  displayName: string;
  image: string;
  isHost: boolean;
  followersCount: number;
  followingCount: number;
  following: boolean;
}
