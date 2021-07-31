import { format } from "date-fns";
import { observer } from "mobx-react-lite";
import React, { useContext } from "react";
import { Card, Tab } from "semantic-ui-react";
import { RootStoreContext } from "../../../app/stores/rootStore";

const Hosting = () => {
  const rootStore = useContext(RootStoreContext);
  const { UserActivitiesByDate, loadingUserActivities } =
    rootStore.profileStore;

  return (
    <Tab.Pane
      loading={loadingUserActivities}
      style={{ border: "none", marging: "0px 0px" }}
    >
      <Card.Group itemsPerRow={4}>
        {UserActivitiesByDate &&
          UserActivitiesByDate.map((userActivity) => (
            <Card
              key={userActivity.id}
              align="center"
              image={`/assets/categoryImages/${userActivity.category}.jpg`}
              header={userActivity.title}
              meta={
                <>
                  {format(userActivity.date, "dd-MMM-YYYY")}
                  {<br />}
                  {format(userActivity.date, "hh:mm a")}
                </>
              }
            />
          ))}
      </Card.Group>
    </Tab.Pane>
  );
};

export default observer(Hosting);
