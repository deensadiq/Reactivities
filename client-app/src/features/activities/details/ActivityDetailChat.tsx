import React, { Fragment, useContext, useEffect } from "react";
import { Link } from "react-router-dom";
import { Segment, Header, Form, Button, Comment } from "semantic-ui-react";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { Form as FinalForm, Field } from "react-final-form";
import TextAreaInput from "../../../app/common/form/TextAreaInput";
import { observer } from "mobx-react-lite";
import { formatDistance } from "date-fns";

const ActivityDetailChat = () => {
  const rootStore = useContext(RootStoreContext);
  const {
    createHubConnection,
    stopHubConnection,
    activity,
    createComment,
  } = rootStore.activityStore;

  useEffect(() => {
    createHubConnection(activity!.id);

    return () => {
      stopHubConnection();
    };
  }, [createHubConnection, stopHubConnection, activity!.id]);

  return (
    <Fragment>
      <Segment
        textAlign="center"
        attached="top"
        inverted
        color="teal"
        style={{ border: "none" }}
      >
        <Header>Chat about this event</Header>
      </Segment>
      <Segment attached>
        <Comment.Group>
          {activity &&
            activity.comments &&
            activity.comments.map((comment) => (
              <Comment>
                <Comment.Avatar src={comment.image || "/assets/user.png"} />
                <Comment.Content>
                  <Comment.Author as={Link} to={`profiles/${comment.username}`}>
                    {comment.displayName}
                  </Comment.Author>
                  <Comment.Metadata>
                    <div>{formatDistance(comment.dateCreated, new Date())}</div>
                  </Comment.Metadata>
                  <Comment.Text>{comment.body}</Comment.Text>
                </Comment.Content>
              </Comment>
            ))}
          <FinalForm
            onSubmit={createComment}
            render={({ handleSubmit, submitting, form }) => (
              <Form onSubmit={() => handleSubmit()!.then(() => form.reset())}>
                <Field
                  name="body"
                  component={TextAreaInput}
                  rows={2}
                  placeholder="Add your comment here!"
                />
                <Button
                  content="Add Reply"
                  labelPosition="left"
                  icon="edit"
                  primary
                  loading={submitting}
                />
              </Form>
            )}
          />
        </Comment.Group>
      </Segment>
    </Fragment>
  );
};

export default observer(ActivityDetailChat);
