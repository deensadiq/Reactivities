import { observer } from "mobx-react-lite";
import React, { useContext, useState } from "react";
import { Button, Grid, Header, Tab } from "semantic-ui-react";
import { RootStoreContext } from "../../app/stores/rootStore";
import ProfileEditForm from "./ProfileEditForm";

const ProfileDescription = () => {
  const rootStore = useContext(RootStoreContext);
  const { profile, IsCurrentUser } = rootStore.profileStore;

  const [editProfile, setEditProfile] = useState(false);

  return (
    <Tab.Pane>
      <Grid>
        <Grid.Column width={16}>
          <Header floated="left" icon="user" content={profile!.displayName} />
          {IsCurrentUser && (
            <Button
              basic
              floated="right"
              content={editProfile ? "Cancel" : "Edit Profile"}
              onClick={() => setEditProfile(!editProfile)}
            />
          )}
        </Grid.Column>
        <Grid.Column width={16}>
          {editProfile ? <ProfileEditForm setEditState={setEditProfile} /> : <div>{profile?.bio}</div>}
        </Grid.Column>
      </Grid>
    </Tab.Pane>
  );
};

export default observer(ProfileDescription);
