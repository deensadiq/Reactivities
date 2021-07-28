import React from "react";
import { List, Image, Popup } from "semantic-ui-react";
import { IAttendee } from "../../../app/models/activity";
import ProfileCard from "../../profiles/ProfileCard";

interface IProps {
  attendees: IAttendee[];
}

const ActivityListItemAttendees: React.FC<IProps> = ({ attendees }) => {
  const styles = {
    borderColor: "orange",
    borderWidth: 2,
  };
  return (
    <List horizontal size="huge" relaxed>
      {attendees.map((attendee) => (
        <List.Item key={attendee.username}>
          <Popup
            header={attendee.displayName}
            position="top left"
            size="small"
            trigger={
              <Image
                avatar
                src={attendee.image || "/assets/user.png"}
                bordered
                style={attendee.following ? styles : null}
              />
            }
          >
            <Popup.Content>
              <ProfileCard attendee={attendee} />
            </Popup.Content>
          </Popup>
        </List.Item>
      ))}
    </List>
  );
};

export default ActivityListItemAttendees;
