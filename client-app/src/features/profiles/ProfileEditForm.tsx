import { observer } from "mobx-react-lite";
import React, { useContext } from "react";
import { Field, Form as FinalForm } from "react-final-form";
import { combineValidators, isRequired } from "revalidate";
import { Button, Form } from "semantic-ui-react";
import TextAreaInput from "../../app/common/form/TextAreaInput";
import TextInput from "../../app/common/form/TextInput";
import { IProfileFormValues } from "../../app/models/profile";
import { RootStoreContext } from "../../app/stores/rootStore";

interface IProps {
    setEditState: (value: boolean) => void;
}

const ProfileEditForm: React.FC<IProps> = ({setEditState}) => {
  const rootStore = useContext(RootStoreContext);
  const {
    profile,
    updateProfile,
    loadingUpdateProfile,
  } = rootStore.profileStore;

  const validate = combineValidators({
      displayName: isRequired("Display Name")
  });

  return (
    <FinalForm
      initialValues={profile!}
      validate={validate}
      onSubmit={(values: IProfileFormValues) => {
        updateProfile(values).then(() => setEditState(false));
      }}
      render={({ handleSubmit }) => (
        <Form onSubmit={() => handleSubmit()}>
          <Field
            name="displayName"
            component={TextInput}
            placeholder="Display Name"
            value={profile!.displayName}
          />
          <Field
            name="bio"
            rows={2}
            component={TextAreaInput}
            placeholder="Biography"
            value={profile!.bio}
          />
          <Button
            loading={loadingUpdateProfile}
            color="green"
            floated="right"
            content="Update Profile"
          />
        </Form>
      )}
    />
  );
};

export default observer(ProfileEditForm);
