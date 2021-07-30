import React from "react";
import { Card, Icon, Image } from "semantic-ui-react";
import { IAttendee } from "../../app/models/activity";

interface IProp {
  attendee: IAttendee;
}

const ProfileCard: React.FC<IProp> = ({ attendee }) => {
  return (
    <Card>
      <Image
        size="small"
        src={attendee.image || "/assets/user.png"}
        wrapped
        ui={false}
      />
      <Card.Content>
        <Card.Header>{attendee.displayName}</Card.Header>
        <Card.Description>
          Matthew is a musician living in Nashville.
        </Card.Description>
      </Card.Content>
      <Card.Content extra>
        <a href={`/profiles/${attendee.username}`}>
          <Icon name="user" />
          {attendee.followersCount} Followers
        </a>
      </Card.Content>
    </Card>
  );
};

export default ProfileCard;
