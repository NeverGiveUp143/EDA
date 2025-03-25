import { Tab, Tabs } from "@mui/material";
import { TabProps } from "./propTypes";
import { useState } from "react";

const TabBar = (props : TabProps) => { 
    const { tabs, defaultTab,screenLoader } = props;
    const [activeTab, setActiveTab] = useState<string>(defaultTab);
    return (
      <>
        <Tabs
          variant={tabs?.length > 4 ? "scrollable" : "standard"}
          scrollButtons="auto"
          value={activeTab}
          onChange={(_event, newValue) => setActiveTab(newValue)}
        >
          {tabs?.map((tab) => {
            return (
              <Tab
                key={tab.id}
                label={tab.label}
                value={tab.id}
                disabled={tab.disabled}
              />
            );
          })}
        </Tabs>
        {screenLoader({ screenId: activeTab as string })}
      </>
    );
}

export default TabBar;