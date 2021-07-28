import { observer } from "mobx-react-lite";
import React, { useContext, useState } from "react";
import { Button, Card, Grid, Header, Image, Tab } from "semantic-ui-react";
import PhotoUploadWidget from "../../app/common/photo/PhotoUploadWidget";
import { RootStoreContext } from "../../app/stores/rootStore";

const ProfilePhotos = () => {
  const rootStore = useContext(RootStoreContext);
  const {
    profile,
    IsCurrentUser,
    uploadingPhoto,
    uploadPhoto,
    deletePhoto,
    deletingPhoto,
    setMainPhoto,
    loading,
  } = rootStore.profileStore;
  const [addPhotoMode, setAddPhotoMode] = useState(false);
  const [target, setTarget] = useState<string | undefined>(undefined);

  const handleUploadPhoto = (file: Blob) => {
    uploadPhoto(file).then(() => setAddPhotoMode(false));
  };

  return (
    <Tab.Pane>
      <Grid>
        <Grid.Column style={{ paddingBottom: 0 }} width={16}>
          <Header floated="left" icon="image" content="Photos" />
          {IsCurrentUser && (
            <Button
              floated="right"
              basic
              content={addPhotoMode ? "Cancel" : "Add Photo"}
              onClick={() => setAddPhotoMode(!addPhotoMode)}
            />
          )}
        </Grid.Column>
        <Grid.Column width={16}>
          {addPhotoMode ? (
            <PhotoUploadWidget
              loading={uploadingPhoto}
              uploadPhoto={handleUploadPhoto}
            />
          ) : (
            <Card.Group itemsPerRow={5}>
              {profile &&
                profile!.photos!.map((photo) => (
                  <Card key={photo.id}>
                    <Image src={photo.url} />
                    {IsCurrentUser && (
                      <Button.Group widths={2}>
                        <Button
                          name={photo.id}
                          loading={loading && target === photo.id}
                          disabled={photo.isMain}
                          onClick={(e) => {
                            setMainPhoto(photo);
                            setTarget(e.currentTarget.name);
                          }}
                          basic
                          color="teal"
                          content="IsMain"
                        />
                        <Button
                          name={photo.id}
                          disabled={photo.isMain}
                          loading={deletingPhoto && target === photo.id}
                          onClick={(e) => {
                            deletePhoto(photo.id);
                            setTarget(e.currentTarget.name);
                          }}
                          basic
                          color="orange"
                          icon="trash"
                        />
                      </Button.Group>
                    )}
                  </Card>
                ))}
            </Card.Group>
          )}
        </Grid.Column>
      </Grid>
    </Tab.Pane>
  );
};

export default observer(ProfilePhotos);
