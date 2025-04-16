
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
  

export default PostApi;