import React from "react";
import { Dimmer, Loader } from "semantic-ui-react";

const LoadingComponent: React.FC<{ inverted?: boolean; content?: string; size?: "tiny" | "mini" | "small" | "medium" | "large" | "big" | "huge" | "massive" | undefined }> = ({
  inverted = true,
  content,
  size = "small"
}) => {
  return (
    <Dimmer active inverted={inverted}>
      <Loader size={size} content={content} />
    </Dimmer>
  );
};

export default LoadingComponent;
