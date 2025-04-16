
const PostApi =  async (url: string, data: any) => {
    try {
      const response = await fetch(url, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
      });
  
      const message = await response.text(); 
  
      if (response.ok && message === "") {
        return { success: true };
      } else {
        return { success: false, error: message || "Unknown error occurred." };
      }
    } catch (error) {
      return { success: false, error: "Network error or server unavailable." };
    }
  };

  const FetchApi =  async (url : string) => {
    try {
        const response = await fetch(url);
        if (!response.ok) throw new Error("Failed to fetch data");
        const result = await response.json();
        if (typeof result === "string") {
          return { success: false, error: result || "Unknown error occurred." }
        } else {
          return { success: true , data : result};
        }
    } catch (err: any) {
      return { success: false, error: "Network error or server unavailable." };
    } 
};

  

const ApiHandler = { PostApi, FetchApi };

export default ApiHandler;