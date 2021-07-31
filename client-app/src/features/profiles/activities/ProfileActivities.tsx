import React, { useContext, useEffect } from "react";
import { Grid, Header, Tab } from "semantic-ui-react";
import { RootStoreContext } from "../../../app/stores/rootStore";
import FutureEvents from "./FutureEvents";
import Hosting from "./Hosting";
import PastEvents from "./PastEvents";

const panes = [
  {
    menuItem: "Future Events",
    render: () => <FutureEvents />,
  },
  { menuItem: "Past Events", render: () => <PastEvents /> },
  { menuItem: "Hosting", render: () => <Hosting /> },
];

const ProfileActivities = () => {
  const rootStore = useContext(RootStoreContext);
  const { setPredicate } = rootStore.profileStore;

  useEffect(() => {
    setPredicate("futureEvents", "true");
  }, [setPredicate]);

  const handleTabChange = (data: any) => {
    // console.log(data);
    switch (data.activeIndex) {
      case 0:
        setPredicate("futureEvents", "true");
        break;
      case 1:
        setPredicate("pastEvents", "true");
        break;
      case 2:
        setPredicate("isHost", "true");
        break;
    }
  };

  return (
    <Tab.Pane>
      <Grid>
        <Grid.Column width={16}>
          <Header icon="slideshare" content="Activities" />
        </Grid.Column>
        <Grid.Column width={16}>
          <Tab
            menu={{ secondary: true, pointing: true, border: false }}
            panes={panes}
            onTabChange={(e, data) => handleTabChange(data)}
          />
        </Grid.Column>
      </Grid>
    </Tab.Pane>
  );
};

export default ProfileActivities;
