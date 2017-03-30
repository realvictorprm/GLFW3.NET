
typedef enum VkResult {
    VK_SUCCESS = 0,
    VK_NOT_READY = 1,
    VK_TIMEOUT = 2,
    VK_EVENT_SET = 3,
    VK_EVENT_RESET = 4,
    VK_INCOMPLETE = 5,
    VK_ERROR_OUT_OF_HOST_MEMORY = -1,
    VK_ERROR_OUT_OF_DEVICE_MEMORY = -2,
    VK_ERROR_INITIALIZATION_FAILED = -3,
    VK_ERROR_DEVICE_LOST = -4,
    VK_ERROR_MEMORY_MAP_FAILED = -5,
    VK_ERROR_LAYER_NOT_PRESENT = -6,
    VK_ERROR_EXTENSION_NOT_PRESENT = -7,
    VK_ERROR_FEATURE_NOT_PRESENT = -8,
    VK_ERROR_INCOMPATIBLE_DRIVER = -9,
    VK_ERROR_TOO_MANY_OBJECTS = -10,
    VK_ERROR_FORMAT_NOT_SUPPORTED = -11,
    VK_ERROR_FRAGMENTED_POOL = -12,
    VK_ERROR_SURFACE_LOST_KHR = -1000000000,
    VK_ERROR_NATIVE_WINDOW_IN_USE_KHR = -1000000001,
    VK_SUBOPTIMAL_KHR = 1000001003,
    VK_ERROR_OUT_OF_DATE_KHR = -1000001004,
    VK_ERROR_INCOMPATIBLE_DISPLAY_KHR = -1000003001,
    VK_ERROR_VALIDATION_FAILED_EXT = -1000011001,
    VK_ERROR_INVALID_SHADER_NV = -1000012000,
    VK_ERROR_OUT_OF_POOL_MEMORY_KHR = -1000069000,
    VK_RESULT_BEGIN_RANGE = VK_ERROR_FRAGMENTED_POOL,
    VK_RESULT_END_RANGE = VK_INCOMPLETE,
    VK_RESULT_RANGE_SIZE = (VK_INCOMPLETE - VK_ERROR_FRAGMENTED_POOL + 1),
    VK_RESULT_MAX_ENUM = 0x7FFFFFFF
} VkResult;

/*! @brief Returns the address of the specified Vulkan instance function.
*
*  This function returns the address of the specified Vulkan core or extension
*  function for the specified instance.  If instance is set to `NULL` it can
*  return any function exported from the Vulkan loader, including at least the
*  following functions:
*
*  - `vkEnumerateInstanceExtensionProperties`
*  - `vkEnumerateInstanceLayerProperties`
*  - `vkCreateInstance`
*  - `vkGetInstanceProcAddr`
*
*  If Vulkan is not available on the machine, this function returns `NULL` and
*  generates a @ref GLFW_API_UNAVAILABLE error.  Call @ref glfwVulkanSupported
*  to check whether Vulkan is available.
*
*  This function is equivalent to calling `vkGetInstanceProcAddr` with
*  a platform-specific query of the Vulkan loader as a fallback.
*
*  @param[in] instance The Vulkan instance to query, or `NULL` to retrieve
*  functions related to instance creation.
*  @param[in] procname The ASCII encoded name of the function.
*  @return The address of the function, or `NULL` if an
*  [error](@ref error_handling) occurred.
*
*  @errors Possible errors include @ref GLFW_NOT_INITIALIZED and @ref
*  GLFW_API_UNAVAILABLE.
*
*  @pointer_lifetime The returned function pointer is valid until the library
*  is terminated.
*
*  @thread_safety This function may be called from any thread.
*
*  @sa @ref vulkan_proc
*
*  @since Added in version 3.2.
*
*  @ingroup vulkan
*/
GLFWAPI GLFWvkproc glfwGetInstanceProcAddress(void* instance, const char* procname);

/*! @brief Returns whether the specified queue family can present images.
*
*  This function returns whether the specified queue family of the specified
*  physical device supports presentation to the platform GLFW was built for.
*
*  If Vulkan or the required window surface creation instance extensions are
*  not available on the machine, or if the specified instance was not created
*  with the required extensions, this function returns `GLFW_FALSE` and
*  generates a @ref GLFW_API_UNAVAILABLE error.  Call @ref glfwVulkanSupported
*  to check whether Vulkan is available and @ref
*  glfwGetRequiredInstanceExtensions to check what instance extensions are
*  required.
*
*  @param[in] instance The instance that the physical device belongs to.
*  @param[in] device The physical device that the queue family belongs to.
*  @param[in] queuefamily The index of the queue family to query.
*  @return `GLFW_TRUE` if the queue family supports presentation, or
*  `GLFW_FALSE` otherwise.
*
*  @errors Possible errors include @ref GLFW_NOT_INITIALIZED, @ref
*  GLFW_API_UNAVAILABLE and @ref GLFW_PLATFORM_ERROR.
*
*  @thread_safety This function may be called from any thread.  For
*  synchronization details of Vulkan objects, see the Vulkan specification.
*
*  @sa @ref vulkan_present
*
*  @since Added in version 3.2.
*
*  @ingroup vulkan
*/
GLFWAPI int glfwGetPhysicalDevicePresentationSupport(void* instance, void* device, uint32_t queuefamily);

/*! @brief Creates a Vulkan surface for the specified window.
*
*  This function creates a Vulkan surface for the specified window.
*
*  If the Vulkan loader was not found at initialization, this function returns
*  `VK_ERROR_INITIALIZATION_FAILED` and generates a @ref GLFW_API_UNAVAILABLE
*  error.  Call @ref glfwVulkanSupported to check whether the Vulkan loader was
*  found.
*
*  If the required window surface creation instance extensions are not
*  available or if the specified instance was not created with these extensions
*  enabled, this function returns `VK_ERROR_EXTENSION_NOT_PRESENT` and
*  generates a @ref GLFW_API_UNAVAILABLE error.  Call @ref
*  glfwGetRequiredInstanceExtensions to check what instance extensions are
*  required.
*
*  The window surface must be destroyed before the specified Vulkan instance.
*  It is the responsibility of the caller to destroy the window surface.  GLFW
*  does not destroy it for you.  Call `vkDestroySurfaceKHR` to destroy the
*  surface.
*
*  @param[in] instance The Vulkan instance to create the surface in.
*  @param[in] window The window to create the surface for.
*  @param[in] allocator The allocator to use, or `NULL` to use the default
*  allocator.
*  @param[out] surface Where to store the handle of the surface.  This is set
*  to `VK_NULL_HANDLE` if an error occurred.
*  @return `VK_SUCCESS` if successful, or a Vulkan error code if an
*  [error](@ref error_handling) occurred.
*
*  @errors Possible errors include @ref GLFW_NOT_INITIALIZED, @ref
*  GLFW_API_UNAVAILABLE and @ref GLFW_PLATFORM_ERROR.
*
*  @remarks If an error occurs before the creation call is made, GLFW returns
*  the Vulkan error code most appropriate for the error.  Appropriate use of
*  @ref glfwVulkanSupported and @ref glfwGetRequiredInstanceExtensions should
*  eliminate almost all occurrences of these errors.
*
*  @thread_safety This function may be called from any thread.  For
*  synchronization details of Vulkan objects, see the Vulkan specification.
*
*  @sa @ref vulkan_surface
*  @sa glfwGetRequiredInstanceExtensions
*
*  @since Added in version 3.2.
n
*  @ingroup vulkan
*/
GLFWAPI VkResult glfwCreateWindowSurface(void* instance, GLFWwindow* window, const void* allocator, int32_t* surface);

/*! @brief Returns the Vulkan instance extensions required by GLFW.
*
*  This function returns an array of names of Vulkan instance extensions required
*  by GLFW for creating Vulkan surfaces for GLFW windows.  If successful, the
*  list will always contains `VK_KHR_surface`, so if you don't require any
*  additional extensions you can pass this list directly to the
*  `VkInstanceCreateInfo` struct.
*
*  If Vulkan is not available on the machine, this function returns `NULL` and
*  generates a @ref GLFW_API_UNAVAILABLE error.  Call @ref glfwVulkanSupported
*  to check whether Vulkan is available.
*
*  If Vulkan is available but no set of extensions allowing window surface
*  creation was found, this function returns `NULL`.  You may still use Vulkan
*  for off-screen rendering and compute work.
*
*  @param[out] count Where to store the number of extensions in the returned
*  array.  This is set to zero if an error occurred.
*  @return An array of ASCII encoded extension names, or `NULL` if an
*  [error](@ref error_handling) occurred.
*
*  @errors Possible errors include @ref GLFW_NOT_INITIALIZED and @ref
*  GLFW_API_UNAVAILABLE.
*
*  @remarks Additional extensions may be required by future versions of GLFW.
*  You should check if any extensions you wish to enable are already in the
*  returned array, as it is an error to specify an extension more than once in
*  the `VkInstanceCreateInfo` struct.
*
*  @pointer_lifetime The returned array is allocated and freed by GLFW.  You
*  should not free it yourself.  It is guaranteed to be valid only until the
*  library is terminated.
*
*  @thread_safety This function may be called from any thread.
*
*  @sa @ref vulkan_ext
*  @sa glfwCreateWindowSurface
*
*  @since Added in version 3.2.
*
*  @ingroup vulkan
*/
//GLFWAPI const char** glfwGetRequiredInstanceExtensions(uint32_t* count);